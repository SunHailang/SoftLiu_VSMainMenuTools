using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class CustomDescEnum : Attribute
    {
        public string dataValidation { get; private set; }
        public CustomDescEnum(string desc = "")
        {
            this.dataValidation = desc;
        }
    }
}
