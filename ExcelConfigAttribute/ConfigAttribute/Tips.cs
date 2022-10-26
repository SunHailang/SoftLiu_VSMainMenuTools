using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Tips : Attribute
    {
        public string description { get; private set; }

        public string collectionTip;

        public Tips(string description)
        {
            this.description = description;
        }
    }
}
