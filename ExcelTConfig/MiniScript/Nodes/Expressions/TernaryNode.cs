using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class TernaryNode : ExpressionNode
    {
        public ExpressionNode condition { get; }
        public ExpressionNode trueState { get; }
        public ExpressionNode falseState { get; }

        public TernaryNode(ExpressionNode condition, ExpressionNode trueState, ExpressionNode falseState)
        {
            this.condition = condition;
            this.trueState = trueState;
            this.falseState = falseState;
        }

        public override string ToString()
        {
            return $"{condition}?{trueState}:{falseState}";
        }

        public override void Visit(OpCodeEngine engine)
        {
            condition.Visit(engine);
            int conditionLabel = engine.PushGoto(OpCode.GoToOnFalse);
            trueState.Visit(engine);
            int trueLabel = engine.PushGoto(OpCode.GoTo);

            int falsePosition = engine.position;
            falseState.Visit(engine);
            int endPosition = engine.position;

            engine.CoverValue(conditionLabel, (ushort)falsePosition);
            engine.CoverValue(trueLabel, (ushort)endPosition);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            trueState.PyVisit(pyEngine);
            pyEngine.AppendSpace().Append("if").AppendSpace();
            condition.PyVisit(pyEngine);
            pyEngine.AppendSpace().Append("else").AppendSpace();
            falseState.PyVisit(pyEngine);
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo(Math.Max(condition.GetNodeInfo().maxStack, Math.Max(trueState.GetNodeInfo().maxStack, falseState.GetNodeInfo().maxStack)));
        }
    }
}
