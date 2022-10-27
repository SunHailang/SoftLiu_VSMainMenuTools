using System;

namespace Config.ExcelConfigAttribute
{
    public enum SplitSingleType
    {
        Int,
        Float,
        String,
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class MarkSplitString: Attribute
    {
        public SplitSingleType singleType {get; private set; }

        public MarkSplitString(SplitSingleType type = SplitSingleType.Int)
        {
            singleType = type;

        }
    }
}
