using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class DeclareStatementNode : StatementNode
    {
        public TypeNode type { get; }
        public VariableNode variable { get; }

        public DeclareStatementNode(TypeNode type, VariableNode variable)
        {
            this.type = type;
            this.variable = variable;
        }

        public override void Visit(OpCodeEngine engine)
        {
            engine.AddLocal(variable.text, variable.codePosition);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            
        }

        public override string ToString()
        {
            return $"{type} {variable};";
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(0);
        }
    }
}
