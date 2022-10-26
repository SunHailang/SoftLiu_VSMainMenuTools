using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ExcelConfigAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class Prompt : Attribute
    {
        public string content { get; private set; }

        public string title;

        public const string DefaultTitle = "ToolTip";

        public Prompt(string content)
        {
            this.content = content;
        }
    }
}
