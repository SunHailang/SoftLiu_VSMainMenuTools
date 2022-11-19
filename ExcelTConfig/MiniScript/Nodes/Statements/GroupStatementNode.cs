using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class GroupStatementNode : StatementNode
    {
        public StatementNode[] statements;

        public GroupStatementNode(StatementNode[] statements)
        {
            this.statements = statements;
        }

        public override string ToString()
        {
            return $"{{{string.Join<StatementNode>(";", statements)}}}";
        }

        public override void Visit(OpCodeEngine engine)
        {
            using (engine.PushScope())
            {
                foreach (var statement in statements)
                {
                    statement.Visit(engine);
                }
            }
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            using (pyEngine.PushScope())
            {
                //pyEngine.indent++;
                foreach (var statement in statements)
                {
                    statement.PyVisit(pyEngine);
                    pyEngine.AppendLine();
                }
                //pyEngine.indent--;
            }
        }

        internal override NodeInfo GetNodeInfo()
        {
            int maxStack;

            if (statements.Length == 0) maxStack = 0;
            else
            {
                maxStack = statements[0].GetNodeInfo().maxStack;

                for(int it = 1; it < statements.Length; it++)
                {
                    maxStack = Math.Max(maxStack, statements[it].GetNodeInfo().maxStack);
                }
            }

            return new NodeInfo { maxStack = maxStack };
        }
    }
}
