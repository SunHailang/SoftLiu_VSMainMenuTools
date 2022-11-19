using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class ReturnStatementNode : StatementNode
    {
        public ExpressionNode expression { get; }

        public ReturnStatementNode(ExpressionNode expression)
        {
            this.expression = expression;
        }

        public override void Visit(OpCodeEngine engine)
        {
            if (expression == null) engine.PushOpCode(OpCode.LoadFalse);
            else expression.Visit(engine);
            engine.PushOpCode(OpCode.Ret);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append("return");
            if(expression != null)
            {
                pyEngine.AppendSpace();
                expression.PyVisit(pyEngine);
            }
        }

        public override string ToString()
        {
            return expression == null ? "return;" : $"return {expression};";
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(expression == null ? 1 : expression.GetNodeInfo().maxStack);
        }
    }
}
