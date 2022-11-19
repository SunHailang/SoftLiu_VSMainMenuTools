using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class BinaryNode : ExpressionNode
    {
        public string text { get; private set; }
        public Node operand1 { get; private set; }
        public Node operand2 { get; private set; }

        public BinaryNode(string text, Node operand1, Node operand2)
        {
            this.text = text;
            this.operand1 = operand1;
            this.operand2 = operand2;
        }

        public override string ToString()
        {
            return $"{operand1}{text}{operand2}";
        }

        public override void Visit(OpCodeEngine engine)
        {
            //短路
            if(text == "&&" || text == "||")
            {
                operand1.Visit(engine);
                engine.PushOpCode(OpCode.Dup);
                int here = engine.PushGoto(text == "&&" ? OpCode.GoToOnFalse : OpCode.GoToOnTrue);
                engine.PushOpCode(OpCode.Pop);
                operand2.Visit(engine);
                int position = engine.position;

                engine.CoverValue(here, (ushort)position);
                return;
            }

            operand1.Visit(engine);
            operand2.Visit(engine);

            switch (text)
            {
                case "+": engine.PushOpCode(OpCode.Add); break;
                case "-": engine.PushOpCode(OpCode.Substract); break;
                case "*": engine.PushOpCode(OpCode.Multify); break;
                case "/": engine.PushOpCode(OpCode.Divide); break;
                case "%": engine.PushOpCode(OpCode.Mods); break;

                case "<": engine.PushOpCode(OpCode.LT); break;
                case "<=": engine.PushOpCode(OpCode.LE); break;
                case "==": engine.PushOpCode(OpCode.EQ); break;
                case "!=": engine.PushOpCode(OpCode.NE); break;
                case ">=": engine.PushOpCode(OpCode.LE); break;
                case ">": engine.PushOpCode(OpCode.GT); break;

                throw new Exception("should not reach here");
            }
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            operand1.PyVisit(pyEngine);

            pyEngine.AppendSpace();
            if (text == "&&") pyEngine.Append("and");
            else if (text == "||") pyEngine.Append("or");
            else pyEngine.Append(text);
            pyEngine.AppendSpace();

            operand2.PyVisit(pyEngine);
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(Util.GetSequenceMaxStack(operand1, operand2));
        }
    }
}
