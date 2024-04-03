using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public abstract class BinaryOperatorNode : Node
    {
        protected Node left;
        protected Node right;

        public BinaryOperatorNode(Node left, Node right)
        {
            this.left = left;
            this.right = right;
        }
    }
}
