using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class BinaryOperatorFactory
    {
        public static BinaryOperatorNode CreateBinaryNode(Node left, Node right, char op)
        {
            BinaryOperatorNode ObjectType = null;

            if (op  == '+')
            {
                ObjectType = new AdditionNode(left, right);
            }
            else if (op == '-')
            {
                ObjectType = new SubtractionNode(left, right);
            }
            else if (op == '*')
            {
                ObjectType = new MultiplicationNode(left, right);
            }
            else if (op == '/')
            {
                ObjectType = new DivisionNode(left, right);
            }

            return ObjectType;

        }
    }
}
