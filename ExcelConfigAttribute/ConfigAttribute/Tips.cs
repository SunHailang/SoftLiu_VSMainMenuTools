using System;

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
