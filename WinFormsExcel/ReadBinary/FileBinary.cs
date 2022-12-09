using System;
using System.Runtime.InteropServices;

namespace WinFormsExcel
{

    [StructLayout(LayoutKind.Sequential)]
    public struct FileHeader
    {
        public int zero;
        public int width;
        public int count;
        public int dataOffset;
        public int idTableOffset;
        public int nameTableOffset;
        public int groupKeyOffset;
    }

    public unsafe class KInfo
    {
        public string name;
        public FileHeader header;
        public IDTable idTable;

        public byte* dataPointer;

        public KInfo(IntPtr ptr)
        {
            dataPointer = (byte*)ptr.ToPointer();

            header = *(FileHeader*)dataPointer;

            if (header.idTableOffset != 0) idTable = new IDTable(dataPointer + header.idTableOffset);

        }

        public byte* ByIndex(int index)
        {
            return index < 0 ? null : dataPointer + header.dataOffset + header.width * index;
        }

        public byte* ByID(int id)
        {
            return ByIndex(idTable.Find(id));
        }
    }
}
