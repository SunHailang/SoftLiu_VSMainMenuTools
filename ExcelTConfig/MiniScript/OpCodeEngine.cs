using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using MiniScript.Nodes;

namespace MiniScript.OpCodes
{
    public unsafe class OpCodeEngine : Engine
    {
        private List<byte> codes = new List<byte>();

        public int position => codes.Count;

        public OpCodeEngine(string[] globalValues, string[] arguments) : base(globalValues, arguments) { }

        public int PushGoto(OpCode opcode)
        {
            int ret = PushOpCode(opcode);
            PushValue(ushort.MinValue);
            return ret;
        }

        public void CoverValue(int offset, byte value)
        {
            codes[offset] = value;
        }

        public void CoverValue(int offset, ushort value)
        {
            byte v0 = (byte)(value & 0xff);
            byte v1 = (byte)((value >> 8) & 0xff);

            codes[offset] = v0;
            codes[offset + 1] = v1;
        }

        public int PushOpCode(OpCode opcode)
        {
            codes.Add((byte)opcode);
            return codes.Count;
        }

        public int PushValue(byte value)
        {
            codes.Add(value);
            return codes.Count;
        }

        public int PushValue(ushort value)
        {
            byte v0 = (byte)(value & 0xff);
            byte v1 = (byte)((value >> 8) & 0xff);
            codes.Add(v0);
            codes.Add(v1);
            return codes.Count;
        }

        public int PushValue(int value)
        {
            byte v0 = (byte)(value & 0xff);
            byte v1 = (byte)((value >> 8) & 0xff);
            byte v2 = (byte)((value >> 16) & 0xff);
            byte v3 = (byte)((value >> 24) & 0xff);
            codes.Add(v0);
            codes.Add(v1);
            codes.Add(v2);
            codes.Add(v3);
            return codes.Count;
        }

        public int PushValue(float value)
        {
            return PushValue(*(int*)&value);
        }

        public byte[] Emit(string content, Node[] nodes)
        {
            SetContent(content);
            codes.Clear();
            PushValue(byte.MinValue); //max local values
            PushValue(byte.MinValue); //max stack
            int maxStack = 0;
            using (PushScope())
            {
                foreach (var node in nodes)
                {
                    node.Visit(this);
                    maxStack = Math.Max(maxStack, node.GetNodeInfo().maxStack);
                }
            }
            PushOpCode(OpCode.Ret);
            CoverValue(0, (byte)maxLocalValuess);
            CoverValue(1, (byte)maxStack);
            SetContent(null);
            return codes.ToArray();
        }

        private struct ForWhileInfo
        {
            public ushort continuePosition;
            public int breakClipFrom;
        }

        private Stack<ForWhileInfo> forWhileStack = new Stack<ForWhileInfo>();
        private List<int> breakClips = new List<int>();

        public void StartForWhile()
        {
            int continuePosition = position;
            forWhileStack.Push(new ForWhileInfo
            {
                continuePosition = (ushort)continuePosition,
                breakClipFrom = breakClips.Count,
            });
        }

        public void EndForWhile()
        {
            int breakPosition = position;
            var info = forWhileStack.Pop();

            for(int it = info.breakClipFrom; it < breakClips.Count; it++)
            {
                CoverValue(breakClips[it], (ushort)breakPosition);
            }
            breakClips.RemoveRange(info.breakClipFrom, breakClips.Count - info.breakClipFrom);
        }

        public void PushContinue()
        {
            int position = PushGoto(OpCode.GoTo);
            CoverValue(position, forWhileStack.Peek().continuePosition);
        }

        public void PushBreak(OpCode opCode)
        {
            int position = PushGoto(opCode);
            breakClips.Add(position);
        }
    }
}
