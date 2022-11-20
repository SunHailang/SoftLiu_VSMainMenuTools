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

        public static void LoadMain()
        {
            string path = $"{Environment.CurrentDirectory}/../../../Main.bytes";
            IntPtr ptr = OpenBytes(path);
            if(ptr == IntPtr.Zero)
            {
                return;
            }
            byte* p = (byte*)ptr.ToPointer();

            MainHeader header = *(MainHeader*)p;
            p += sizeof(MainHeader);
            for (int it = 0; it < header.count; it++)
            {
                int nameLen = *(ushort*)p;
                p += sizeof(ushort);

                string name = Encoding.UTF8.GetString(p, nameLen);
                p += nameLen;

                int order = *(int*)p;
                p += sizeof(int);

            }
        }


        public struct MainHeader
        {
            public int zero;
            public int count;
            public int maxLen;
        }
    }
}
