using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class FunctionCallStatementNode : StatementNode
    {
        public FunctionCallNode func;

        public FunctionCallStatementNode(FunctionCallNode func)
        {
            this.func = func;
        }

        public override string ToString() => func.ToString();

        public override void Visit(OpCodeEngine engine)
        {
            func.Visit(engine);
            engine.PushOpCode(OpCode.Pop);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            func.PyVisit(pyEngine);
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo
            {
                maxStack = func.GetNodeInfo().maxStack
            };
        }
    }
}
