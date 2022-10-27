using System;

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
