using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniScript
{
    internal struct NodeInfo
    {
        public int maxStack;

        public NodeInfo(int maxStack)
        {
            this.maxStack = maxStack;
        }
    }
}
