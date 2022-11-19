using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public abstract class Node
    {
        public abstract void Visit(OpCodeEngine engine);
        public virtual void PyVisit(PythonEngine pyEngine) { throw new NotImplementedException(); }

        internal abstract NodeInfo GetNodeInfo();

        internal int codePosition;
    }
}
