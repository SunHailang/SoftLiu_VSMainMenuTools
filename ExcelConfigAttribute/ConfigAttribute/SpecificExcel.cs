using System;

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
