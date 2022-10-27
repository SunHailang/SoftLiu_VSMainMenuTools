using System;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CommentOnly : Attribute
    {
        public const string TypeName = "[C]";
    }
}
