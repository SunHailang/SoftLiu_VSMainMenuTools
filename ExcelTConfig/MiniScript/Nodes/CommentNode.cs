using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.OpCodes;

namespace MiniScript.Nodes
{
    public class CommentNode : Node
    {
        public string text { get; }
        public bool singleLineComment { get; }

        public CommentNode(string text, bool singleLineComment)
        {
            this.text = text;
            this.singleLineComment = singleLineComment;
        }

        public override string ToString()
        {
            return singleLineComment ? $"//{text}" : $"/* {text} */";
        }

        public override void Visit(OpCodeEngine engine)
        {
            
        }

        internal override NodeInfo GetNodeInfo()
        {
            throw new NotImplementedException();
        }
    }
}
