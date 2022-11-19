using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class BooleanNode : ExpressionNode
    {
        public string text { get; }

        public BooleanNode(string text)
        {
            this.text = text;
        }

        public override string ToString() => text.ToLower();

        public override void Visit(OpCodeEngine engine)
        {
            engine.PushOpCode(bool.TrueString.Equals(text, StringComparison.OrdinalIgnoreCase) ? OpCode.LoadTrue : OpCode.LoadFalse);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append(bool.TrueString.Equals(text, StringComparison.OrdinalIgnoreCase) ? "True" : "False");
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo { maxStack = 1 };
        }
    }
}
