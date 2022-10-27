using System;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AsTimeStamp : Attribute
    {
        public const string TypeName = "[TimeStamp]";
    }
}
