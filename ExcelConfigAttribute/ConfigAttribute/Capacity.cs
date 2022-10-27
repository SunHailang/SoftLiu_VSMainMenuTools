using System;

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
