using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class IfStatementNode : StatementNode
    {
        public ExpressionNode condition;
        public StatementNode trueState;
        public StatementNode falseState;

        public IfStatementNode(ExpressionNode condition, StatementNode trueState, StatementNode falseState)
        {
            this.condition = condition;
            this.trueState = trueState;
            this.falseState = falseState;
        }

        public override string ToString()
        {
            if (falseState == null) return $"if({condition}) {trueState};";
            else return $"if({condition}) {trueState}; else {falseState};";
        }

        public override void Visit(OpCodeEngine engine)
        {
            condition.Visit(engine);
            int conditionLabel = engine.PushGoto(OpCode.GoToOnFalse);
            trueState.Visit(engine);

            if(falseState == null)
            {
                int falsePosition = engine.position;
                engine.CoverValue(conditionLabel, (ushort)falsePosition);
            }
            else
            {
                int trueLabel = engine.PushGoto(OpCode.GoTo);

                int falsePosition = engine.position;
                falseState.Visit(engine);
                int endPosition = engine.position;

                engine.CoverValue(conditionLabel, (ushort)falsePosition);
                engine.CoverValue(trueLabel, (ushort)endPosition);
            }
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append("if").AppendSpace();
            condition.PyVisit(pyEngine);
            pyEngine.Append(":");
            pyEngine.indent++;
            pyEngine.AppendLine();
            trueState.PyVisit(pyEngine);
            pyEngine.indent--;
            pyEngine.AppendLine();

            if (falseState == null) return;

            if(falseState is IfStatementNode)
            {
                pyEngine.Append("el");
                falseState.PyVisit(pyEngine);
            }
            else
            {
                pyEngine.Append("else:");
                pyEngine.indent++;
                pyEngine.AppendLine();
                falseState.PyVisit(pyEngine);
                pyEngine.indent--;
                pyEngine.AppendLine();
            }
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo
            {
                maxStack = Math.Max(
                    condition.GetNodeInfo().maxStack, 
                    Math.Max(trueState.GetNodeInfo().maxStack, falseState == null ? 0 : falseState.GetNodeInfo().maxStack)
                )
            };
        }
    }
}
