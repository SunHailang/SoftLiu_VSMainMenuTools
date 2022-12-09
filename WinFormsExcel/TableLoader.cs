using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFormsExcel
{
    public unsafe class TableLoader
    {
        private static List<IntPtr> handles = new List<IntPtr>();

        public static KInfo[] kInfo;
        public static Dictionary<string, int> kMap;
        private static IntPtr OpenBytes(string path)
        {
            if (!File.Exists(path))
            {
                return IntPtr.Zero;
            }
            byte[] bytes = File.ReadAllBytes(path);
            IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            handles.Add(ptr);
            return ptr;
        }
        private static IntPtr OpenFile(string name)
        {
            var bytes = File.ReadAllBytes($"{WinFormsExcel.configPath}/BinaryFolderPath/{name}.bytes");
            var ptr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            handles.Add(ptr);

            byte* p = (byte*)ptr.ToPointer();
            FileHeader header = *(FileHeader*)p;


            p += sizeof(FileHeader);

            return ptr;
        }

        public static void LoadMain()
        {
            string path = $"{WinFormsExcel.configPath}/BinaryFolderPath/Main.bytes";
            ExcelTConfig.Entry.UpdateLogInfo($"LoadMain path:{path}");
            IntPtr ptr = OpenBytes(path);
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            byte* p = (byte*)ptr.ToPointer();

            MainHeader header = *(MainHeader*)p;

            Console.WriteLine($"{header.zero}, {header.count}, {header.maxLen}");

            p += sizeof(MainHeader);

            kInfo = new KInfo[header.maxLen];
            kMap = new Dictionary<string, int>(header.maxLen * 2);

            for (int it = 0; it < header.count; it++)
            {
                int nameLen = *(ushort*)p;
                p += sizeof(ushort);

                string name = Encoding.UTF8.GetString(p, nameLen);
                p += nameLen;

                int order = *(int*)p;
                p += sizeof(int);

                kMap[name] = order;
                IntPtr infoPtr = OpenFile(name);
                KInfo info = new KInfo(infoPtr);
                kInfo[order] = info;
            }
            Framework.TableIndex.SetData();

            Framework.TestData data = Framework.TestData.ByIndex(2);

            Console.WriteLine(data.arrayInt);
        }


        
        

        
    }
}
