using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class VariableNode : ExpressionNode
    {
        public string text { get; private set; }

        public VariableType variableType { get; private set; }

        public VariableNode(string text)
        {
            this.text = text;
        }

        public override string ToString() => text;

        private void CheckTypeAndIndex(Engine engine, out VariableType type, out int index)
        {
            index = engine.IndexArgument(text);
            if (index != -1)
            {
                type = VariableType.Argument;
                return;
            }

            index = engine.IndexLocal(text);
            if (index != -1)
            {
                type = VariableType.LocalVariable;
                return;
            }

            index = engine.IndexGlobal(text);
            if (index != -1)
            {
                type = VariableType.GlobalVariable;
                return;
            }

            throw new Exception($"unknown variable name [{text}], {engine.GetCodePositionInfo(codePosition)}");
        }

        public void VisitStore(OpCodeEngine engine)
        {
            VariableType type;
            int index;
            CheckTypeAndIndex(engine, out type, out index);

            switch(type)
            {
                case VariableType.Argument: engine.PushOpCode(OpCode.StoreArgument);break;
                case VariableType.LocalVariable: engine.PushOpCode(OpCode.StoreLocalVariable); break;
                case VariableType.GlobalVariable: engine.PushOpCode(OpCode.StoreGlobalVariable); break;
                default: throw new Exception("should not reach here");
            }
            engine.PushValue((byte)index);
        }

        public void VisitLoad(OpCodeEngine engine)
        {
            VariableType type;
            int index;
            CheckTypeAndIndex(engine, out type, out index);

            switch (type)
            {
                case VariableType.Argument: engine.PushOpCode(OpCode.LoadArgument); break;
                case VariableType.LocalVariable: engine.PushOpCode(OpCode.LoadLocalVariable); break;
                case VariableType.GlobalVariable: engine.PushOpCode(OpCode.LoadGlobalVariable); break;
                default: throw new Exception("should not reach here");
            }
            engine.PushValue((byte)index);
        }

        public void VisitCall(OpCodeEngine engine)
        {
            VariableType type;
            int index;
            CheckTypeAndIndex(engine, out type, out index);

            switch (type)
            {
                case VariableType.Argument: engine.PushOpCode(OpCode.CallArgument); break;
                case VariableType.LocalVariable: throw new Exception("Call Local not support");
                case VariableType.GlobalVariable: engine.PushOpCode(OpCode.CallGlobal); break;
                default: throw new Exception("should not reach here");
            }
            engine.PushValue((byte)index);
        }

        public override void Visit(OpCodeEngine engine)
        {
            VariableType type;
            int index;
            CheckTypeAndIndex(engine, out type, out index);
            VisitLoad(engine);
        }

        public override void PyVisit(PythonEngine pyEngine)
        {
            pyEngine.Append(text);
        }

        internal override NodeInfo GetNodeInfo()
        {
            return new NodeInfo { maxStack = 1 };
        }
    }
}
