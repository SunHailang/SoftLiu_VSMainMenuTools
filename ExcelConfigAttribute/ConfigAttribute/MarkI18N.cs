using System;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MarkI18N : Attribute
    {
        public const string TypeName = "[I18N]";
    }
}
