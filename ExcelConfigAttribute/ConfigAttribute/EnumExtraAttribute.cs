using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 输入的时候增加枚举的额外注解
/// </summary>

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumExtraAttribute : Attribute
    {
        public string extraAttrs;

        public EnumExtraAttribute(string attrs)
        {
            this.extraAttrs = attrs;
        }

        public override string ToString()
        {
            string[] attrs = this.extraAttrs.Split(',');
            string ret = "";
            foreach (var attr in attrs)
            {
                ret += "\n\t" + attr;
            }

            return ret;
        }
    }
}
