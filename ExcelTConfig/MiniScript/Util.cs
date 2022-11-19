using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniScript.Nodes;

namespace MiniScript
{
    internal static class Util
    {
        public static int GetSequenceMaxStack(Node node1, Node node2)
        {
            return Math.Max(node1.GetNodeInfo().maxStack, node2.GetNodeInfo().maxStack + 1);
        }

        public static int GetSequenceMaxStack(Node node1, Node node2, Node node3)
        {
            return Math.Max(node1.GetNodeInfo().maxStack, GetSequenceMaxStack(node2, node3) + 1);
        }

        public static int GetSequenceMaxStack(Node node1, Node node2, Node node3, Node node4)
        {
            return Math.Max(node1.GetNodeInfo().maxStack, GetSequenceMaxStack(node2, node3, node4) + 1);
        }

        public static int GetSequenceMaxStack(params Node[] nodes)
        {
            if (nodes.Length == 0) return 0;
            if (nodes.Length == 1) return nodes[0].GetNodeInfo().maxStack;

            int maxStack = GetSequenceMaxStack(nodes[nodes.Length - 2], nodes[nodes.Length - 1]);
            for(int it = nodes.Length - 3; it >= 0; it--)
            {
                maxStack = Math.Max(nodes[it].GetNodeInfo().maxStack, maxStack + 1);
            }
            return maxStack;
        }
    }
}
