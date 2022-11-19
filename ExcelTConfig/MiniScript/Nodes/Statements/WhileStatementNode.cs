using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class WhileStatementNode : Node
    {
        public ExpressionNode condition { get; }
        public StatementNode loopBody { get; }

        public WhileStatementNode(ExpressionNode condition, StatementNode loopBody)
        {
            this.condition = condition;
            this.loopBody = loopBody;
        }

        public override string ToString()
        {
            return $"while({condition}) {loopBody}";
        }

        public override void Visit(OpCodeEngine engine)
        {
            //continue point                                                          break point
            //(condition) (false? goto break point) (loop body) (goto continue point) (end)
            //---------------------------------------------------------------------
            engine.StartForWhile();
            {
                condition.Visit(engine);
                engine.PushBreak(OpCode.GoToOnFalse);
                loopBody.Visit(engine);
                engine.PushContinue();
            }
            engine.EndForWhile();
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append("while").AppendSpace();
            condition.PyVisit(pyEngine);
            pyEngine.Append(":");
            pyEngine.indent++;
            pyEngine.AppendLine();
            loopBody.PyVisit(pyEngine);
            pyEngine.indent--;
            pyEngine.AppendLine();
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo
            {
                maxStack = Math.Max(condition.GetNodeInfo().maxStack, loopBody.GetNodeInfo().maxStack)
            };
        }
    }
}
