using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TargetExcel : Attribute
    {
        public string sheet;
        public string excel { get; private set; }

        public TargetExcel(string excel)
        {
            this.excel = excel;
        }
    }
}
