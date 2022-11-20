using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework
{
    public unsafe struct VTable
    {
        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* LoadVTable(string name, void** dataPointer);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* VTableFind(IntPtr vTable, IntPtr utf16Key, int keyLen);

        private enum Tag
        {
            None = 0,
            Integer = 1,
            Float = 2,
            String = 4,
            Array = 0x80,
        }

        private IntPtr handle;
        private byte* dataPointer;
        private int version;
        private string tableName;

        private VTable(string tableName, IntPtr handle, byte* dataPointer, int version)
        {
            this.handle = handle;
            this.dataPointer = dataPointer;
            this.version = version;
            this.tableName = tableName;
        }

        public static VTable Load(string name)
        {
            void* dataPointer = null;
            IntPtr handle = (IntPtr)LoadVTable(name, &dataPointer);
            if (handle == IntPtr.Zero)
            {
                throw new Exception($"load vtable [{name}] fail!");
            }
            return new VTable(name, handle, (byte*)dataPointer, Table.GetTableVersion());
        }

        private byte* TryGet(string key, out Tag tag)
        {
            Table.VersionCheck(version);
            fixed (char* pointer = key)
            {
                var p = (byte*)VTableFind(handle, (IntPtr)pointer, key.Length);

                if (p == null)
                {
                    tag = Tag.None;
                    return null;
                }

                tag = (Tag)(*(int*)p);
                return p + sizeof(int);
            }
        }

        private byte* Get(string key, Tag expectedTag)
        {
            Tag tag;
            var p = TryGet(key, out tag);
            if (p == null) throw new Exception($"could not find key [{key}] in vtable [{tableName}]");

            if (tag != expectedTag)
            {
                throw new Exception($"tag not correct: vtable [{tableName}], key [{key}], tag [{tag}], pass in tag [{expectedTag}]");
            }
            return p;
        }

        public int GetInt(string key)
        {
            return *(int*)Get(key, Tag.Integer);
        }

        public float GetFloat(string key)
        {
            int iValue = *(int*)Get(key, Tag.Float);
            return *(float*)&iValue;
        }

        public NString GetString(string key)
        {
            return new NString(dataPointer + *(int*)Get(key, Tag.String), version);
        }

        public NArray<int> GetIntArray(string key)
        {
            return new NArray<int>(Get(key, Tag.Integer | Tag.Array), version);
        }

        public NArray<float> GetFloatArray(string key)
        {
            return new NArray<float>(Get(key, Tag.Float | Tag.Array), version);
        }

        public NStructArray<NString> GetStringArray(string key)
        {
            return new NStructArray<NString>(Get(key, Tag.String | Tag.Array), dataPointer, version);
        }

        public bool ContainsKey(string key)
        {
            Tag tag;
            return TryGet(key, out tag) != null;
        }
    }
}
