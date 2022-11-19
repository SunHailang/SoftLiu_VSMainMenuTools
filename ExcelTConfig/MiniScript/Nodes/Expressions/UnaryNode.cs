using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class UnaryNode : ExpressionNode
    {
        public string text { get; private set; }
        public ExpressionNode operand { get; private set; }

        public UnaryNode(string text, ExpressionNode operand)
        {
            this.text = text;
            this.operand = operand;
        }

        public override string ToString()
        {
            return $"{text}{operand}";
        }

        public override void Visit(OpCodeEngine engine)
        {
            operand.Visit(engine);

            switch (text)
            {
                case "-": engine.PushOpCode(OpCode.Negate); break;
                case "!": engine.PushOpCode(OpCode.Not); break;

                default:throw new Exception("should not reach here");
            }
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            switch (text)
            {
                case "-": pyEngine.Append(text); break;
                case "!": pyEngine.Append("not").AppendSpace(); break;

                default: throw new Exception("should not reach here");
            }
            operand.PyVisit(pyEngine);
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(operand.GetNodeInfo().maxStack);
        }
    }
}
