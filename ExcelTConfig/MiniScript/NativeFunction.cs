using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Value = MiniScript.VM.Value;

namespace MiniScript
{
    public struct CallInfo : IEnumerable<Value>
    {
        internal Value[] stack;
        internal int from, to;
        public int argCount => to - from;
        public Value this[int index]
        {
            get
            {
                if ((uint)index >= argCount) throw new IndexOutOfRangeException();
                return stack[from + index];
            }
        }

        public Value returnValue;

        public IEnumerator<Value> GetEnumerator()
        {
            for (int it = 0; it < argCount; it++) yield return this[it];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int it = 0; it < argCount; it++) yield return this[it];
        }
    }

    public delegate void NativeFunction(ref CallInfo callInfo);
}
