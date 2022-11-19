using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.Nodes;

namespace MiniScript
{
    public class PythonEngine : Engine
    {
        private const string IndentString = "    ";
        private static string[] IndentStrings;
        static PythonEngine()
        {
            IndentStrings = new string[32];
            var str = string.Empty;
            for(int it = 0; it < IndentStrings.Length; it++)
            {
                IndentStrings[it] = str;
                str += IndentString;
            }
        }
        private StringBuilder sb = new StringBuilder();

        public PythonEngine(string[] globalValues, string[] arguments) : base(globalValues, arguments) { }
        public PythonEngine Append<T>(T v) { sb.Append(v.ToString()); return this; }
        public PythonEngine AppendLine() { sb.AppendLine().Append(IndentStrings[indent]); return this; }
        public PythonEngine AppendSpace() { sb.Append(" "); return this; }
        public int indent;

        public string Emit(string content, Node[] nodes)
        {
            SetContent(content);
            sb.Clear();
            indent = 0;

            foreach(var node in nodes)
            {
                node.PyVisit(this);
                AppendLine();
            }

            SetContent(null);
            return sb.ToString();
        }
    }
}
