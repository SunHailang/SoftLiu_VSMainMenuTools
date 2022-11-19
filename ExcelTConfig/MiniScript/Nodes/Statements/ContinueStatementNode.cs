using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class ContinueStatementNode : StatementNode
    {
        private ContinueStatementNode() { }
        private static ContinueStatementNode instance;
        public static ContinueStatementNode Instance
        {
            get
            {
                return instance ?? (instance = new ContinueStatementNode());
            }
        }

        public override void Visit(OpCodeEngine engine)
        {
            engine.PushContinue();
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append("continue");
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(0);
        }
    }
}
