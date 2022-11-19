using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class FunctionCallNode : ExpressionNode
    {
        public VariableNode name { get; }
        public ExpressionNode[] arguments { get; }

        public FunctionCallNode(VariableNode name, ExpressionNode[] arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public override string ToString()
        {
            return $"{name}({string.Join<ExpressionNode>(",", arguments)})";
        }

        public override void Visit(OpCodeEngine engine)
        {
            foreach (var argument in arguments) argument.Visit(engine);
            name.VisitCall(engine);
            engine.PushValue((byte)arguments.Length);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            name.PyVisit(pyEngine);
            pyEngine.Append("(");
            for(int it = 0; it < arguments.Length; it++)
            {
                if (it != 0) pyEngine.Append(", ");
                arguments[it].PyVisit(pyEngine);
            }
            pyEngine.Append(")");
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo
            {
                maxStack = Math.Max(Util.GetSequenceMaxStack(arguments), arguments.Length + 1)
            };
        }
    }
}
