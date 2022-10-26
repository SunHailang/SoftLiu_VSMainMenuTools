using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    public enum SpecificType
    {
        INT,
        TINYINT,
        SMALLINT,
        MEDIUMINT,
        FLOAT,
        VARCHAR,
        CHAR,

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class MarkSpecificType : Attribute
    {
        public SpecificType specificType { get; private set; }
        public int len;
        public bool bNegative;

        public MarkSpecificType(SpecificType type, int vcharLen = 0, bool bNegative = true)
        {
            specificType = type;
            len = vcharLen;
            this.bNegative = bNegative;
        }
    }
}
