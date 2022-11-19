using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class ForStatementNode : Node
    {
        public DeclareStatementNode declare;
        public StatementNode init;
        public ExpressionNode condition;
        public StatementNode loopHead;

        public StatementNode loopBody;

        public ForStatementNode(DeclareStatementNode declare, StatementNode init, ExpressionNode condition, StatementNode loopHead, StatementNode loopBody)
        {
            this.declare = declare;
            this.init = init;
            this.condition = condition;
            this.loopHead = loopHead;
            this.loopBody = loopBody;
        }

        public override void Visit(OpCodeEngine engine)
        {
            using (engine.PushScope())
            {
                if (declare != null) declare.Visit(engine);
                init.Visit(engine);
                int conditionLabel = engine.PushGoto(OpCode.GoTo);
                int conditionPosition;

                engine.StartForWhile();
                {
                    loopHead.Visit(engine);
                    conditionPosition = engine.position;
                    condition.Visit(engine);
                    engine.PushBreak(OpCode.GoToOnFalse);
                    loopBody.Visit(engine);
                    engine.PushContinue();
                }
                engine.EndForWhile();

                engine.CoverValue(conditionLabel, (ushort)conditionPosition);
            }
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            init.PyVisit(pyEngine);
            pyEngine.AppendLine();
            pyEngine.Append("first = True").AppendLine();
            pyEngine.Append("while True:");
            pyEngine.indent++;
            pyEngine.AppendLine();

            pyEngine.Append("if").AppendSpace();
            condition.PyVisit(pyEngine);
            pyEngine.Append(":");
            pyEngine.indent++;
            pyEngine.AppendLine();
            {
                pyEngine.Append("if first: first = False").AppendLine();
                pyEngine.Append("else: ");
                loopHead.PyVisit(pyEngine);
                pyEngine.AppendLine();

                loopBody.PyVisit(pyEngine);
            }
            pyEngine.indent--;
            pyEngine.AppendLine();
            pyEngine.Append("else: break");

            pyEngine.indent--;
            pyEngine.AppendLine();
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo
            {
                maxStack = Math.Max(
                    Math.Max(init.GetNodeInfo().maxStack, condition.GetNodeInfo().maxStack),
                    Math.Max(loopHead.GetNodeInfo().maxStack, loopBody.GetNodeInfo().maxStack)
                )
            };
        }
    }
}
