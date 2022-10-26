using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
