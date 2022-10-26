using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
