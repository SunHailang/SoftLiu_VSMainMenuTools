using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFormsExcel
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MainHeader
    {
        public int zero;
        public int count;
        public int maxLen;
        public int packInfoOffset;
        public int vmapOffset;
    }
    

    public unsafe class IDTable
    {
        public struct Entry
        {
            public int id;
            public int value;
            public int next;
        }

        private Entry[] entries;

        public IDTable(byte* p)
        {
            int length = *(int*)p;
            p += sizeof(int);

            entries = new Entry[length];
            for (int it = 0; it < length; it++)
            {
                entries[it] = ((Entry*)p)[it];
            }
        }

        public int Find(int id)
        {
            int slotIndex = (int)((uint)id % entries.Length);
            Entry* entry;
            fixed (Entry* slots = entries)
            {
                do
                {
                    entry = slots + slotIndex;
                    if (entry->id == id) return entry->value;
                    slotIndex = entry->next;
                }
                while (slotIndex >= 0);
            }
            return -1;
        }
    }

}
