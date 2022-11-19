using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class AssignStatementNode : StatementNode
    {
        public VariableNode variable { get; }
        public ExpressionNode expression { get; }

        public AssignStatementNode(VariableNode variable, ExpressionNode expression)
        {
            this.variable = variable;
            this.expression = expression;
        }

        public override void Visit(OpCodeEngine engine)
        {
            expression.Visit(engine);
            variable.VisitStore(engine);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            variable.PyVisit(pyEngine);
            pyEngine.Append(" = ");
            expression.PyVisit(pyEngine);
        }

        public override string ToString()
        {
            return $"{variable} = {expression};";
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(expression.GetNodeInfo().maxStack);
        }
    }
}
