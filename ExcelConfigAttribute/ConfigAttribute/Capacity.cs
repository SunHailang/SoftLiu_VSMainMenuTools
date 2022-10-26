using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Capacity : Attribute
    {
        public int capacity { get; private set; }

        public Capacity(int capacity)
        {
            this.capacity = capacity;
        }

    }
}
