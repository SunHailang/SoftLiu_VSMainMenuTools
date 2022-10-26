using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IntegerRange : Attribute
    {
        public int min = int.MinValue;
        public int max = int.MaxValue;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class FloatRange : Attribute
    {
        public float min = float.MinValue;
        public float max = float.MaxValue;
    }
}
