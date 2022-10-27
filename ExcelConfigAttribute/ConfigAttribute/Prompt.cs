using System;

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
