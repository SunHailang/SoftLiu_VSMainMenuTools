using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SpecificExcel : Attribute
    {
        public string sheet;
        public string excel { get; private set; }

        public SpecificExcel(string excel)
        {
            this.excel = excel;
        }
    }
}
