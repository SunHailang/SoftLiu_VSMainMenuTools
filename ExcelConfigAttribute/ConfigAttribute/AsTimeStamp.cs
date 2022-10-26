﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AsTimeStamp : Attribute
    {
        public const string TypeName = "[TimeStamp]";
    }
}
