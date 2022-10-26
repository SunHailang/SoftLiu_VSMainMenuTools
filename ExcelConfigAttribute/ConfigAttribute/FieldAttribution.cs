using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    public enum AttributionType
    {
        ClientOnly,
        ServerOnly,
        ClientAndServer
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class FieldAttribution: Attribute
    {
       public  AttributionType type{ get; private set; }
        public FieldAttribution(AttributionType type)
        {
            this.type = type;
        }
    }
}
