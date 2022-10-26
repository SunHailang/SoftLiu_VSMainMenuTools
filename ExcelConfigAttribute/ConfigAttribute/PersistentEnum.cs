using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class PersistentEnum : Attribute
    {
        public string dataValidation;
    }
}
