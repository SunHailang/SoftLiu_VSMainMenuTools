using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class BreakStatementNode : StatementNode
    {
        private BreakStatementNode() { }
        private static BreakStatementNode instance;
        public static BreakStatementNode Instance
        {
            get
            {
                return instance ?? (instance = new BreakStatementNode());
            }
        }

        public override void Visit(OpCodeEngine engine)
        {
            engine.PushBreak(OpCode.GoTo);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append("break");
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(0);
        }
    }
}
