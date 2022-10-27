using System;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Functor : Attribute
    {
        public string[] argNames { get; private set; }

        public Functor(params string[] argNames)
        {
            this.argNames = argNames;
        }
    }
}
