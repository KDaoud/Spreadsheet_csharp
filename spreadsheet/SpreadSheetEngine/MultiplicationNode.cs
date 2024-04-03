using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class MultiplicationNode : BinaryOperatorNode
    {
        public MultiplicationNode(Node left, Node right) : base(left, right) { }

        public override double Evaluate()
        {
            return left.Evaluate() * right.Evaluate();
        }
    }
}
