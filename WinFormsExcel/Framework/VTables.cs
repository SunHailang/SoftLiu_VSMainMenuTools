using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public unsafe class VTables
    {
        public string ComponentName => nameof(VTables);

        public static VTable Load(string name) { return VTable.Load(name); }

        public static VTable Global { get; private set; }

        public VTables()
        {
            Global = Load(nameof(Global));
        }

        internal void Disable() { }
        internal void Reset() { }
    }
}
