using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class UsedInConfig : Attribute
    {
        public string targetNamespace { get; private set; }

        public UsedInConfig(string targetNamespace)
        {
            this.targetNamespace = targetNamespace;
        }
    }
}
