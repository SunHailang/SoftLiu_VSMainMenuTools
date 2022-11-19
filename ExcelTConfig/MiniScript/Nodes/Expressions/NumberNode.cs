using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class NumberNode : ExpressionNode
    {
        public string text { get; private set; }

        public NumberNode(string text)
        {
            this.text = text;
        }

        public override string ToString() => text;

        public override void Visit(OpCodeEngine engine)
        {
            int intValue;
            float floatValue;

            if (int.TryParse(text, out intValue))
            {
                if ((sbyte)intValue == intValue)
                {
                    engine.PushOpCode(OpCode.LoadInteger8bit);
                    engine.PushValue((byte)(sbyte)intValue);
                }
                else if ((short)intValue == intValue)
                {
                    engine.PushOpCode(OpCode.LoadInteger16bit);
                    engine.PushValue((ushort)(short)intValue);
                }
                else
                {
                    engine.PushOpCode(OpCode.LoadInteger32bit);
                    engine.PushValue(intValue);
                }
            }
            else if (float.TryParse(text, out floatValue))
            {
                engine.PushOpCode(OpCode.LoadFloat);
                engine.PushValue(floatValue);
            }
            else throw new Exception("should not reach here");
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append(text);
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo { maxStack = 1 };
        }
    }
}
