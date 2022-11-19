using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class TypeNode : Node
    {
        public string text { get; }

        public TypeNode(string text)
        {
            this.text = text;
        }

        public override void Visit(OpCodeEngine engine) => throw new Exception();

        public override string ToString() => text;

        internal override NodeInfo GetNodeInfo()
        {
            throw new NotImplementedException();
        }
    }
}
