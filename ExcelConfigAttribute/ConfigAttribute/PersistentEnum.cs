using System;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class PersistentEnum : Attribute
    {
        public string dataValidation;
        public PersistentEnum(string data = "")
        {
            this.dataValidation = data;
        }
    }
}
