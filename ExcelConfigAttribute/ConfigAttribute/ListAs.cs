using System;

namespace Config.ExcelConfigAttribute
{
    public enum ListType
    {
        IDKeyMap,
        List,
        CodeNameKeyMap,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ListAs : Attribute
    {
        public ListType listType { get; private set; }
        public ListAs(ListType listType)
        {
            this.listType = listType;
        }
    }
}
