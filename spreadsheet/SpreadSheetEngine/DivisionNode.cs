﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class DivisionNode : BinaryOperatorNode
    {
        public DivisionNode(Node left, Node right) : base(left, right) { }

        public override double Evaluate()
        {
            return left.Evaluate() / right.Evaluate();
        }
    }
}
