﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    public enum ScriptType
    {
        None = 0,
        CSharp = 1 << 0,
        Lua = 1 << 1,
        ALL = ~None,
    }

    public class ExportScripts : Attribute
    {
        public ScriptType type { get; private set; }
        public ExportScripts(ScriptType type)
        {
            this.type = type;
        }
    }
}
