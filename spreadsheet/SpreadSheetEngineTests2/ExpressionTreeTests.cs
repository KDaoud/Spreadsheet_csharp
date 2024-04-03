using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadSheetEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine.Tests
{
    [TestClass()]
    public class ExpressionTreeTests
    {
        [TestMethod()]
        public void EvaluateConstant()
        {
            Node root = new ConstantNode(5);
            ExpressionTree tree = new ExpressionTree(root);
            double result = tree.Evaluate();
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        public void EvaluateVariable()
        {
            Dictionary<string, double> variables = new Dictionary<string, double>
            {
                { "x", 4 }
            };
            Node root = new VariableNode("x", ref variables);
            ExpressionTree tree = new ExpressionTree(root);
            double result = tree.Evaluate();
            Assert.AreEqual(4, result);
        }

        [TestMethod()]
        public void EvaluateAddition()
        {
            Node root = new AdditionNode(new ConstantNode(2), new ConstantNode(3));
            ExpressionTree tree = new ExpressionTree(root);

            double result = tree.Evaluate();
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        public void EvaluateSubstraction()
        {
            Node root = new SubtractionNode(new ConstantNode(5), new ConstantNode(3));
            ExpressionTree tree = new ExpressionTree(root);

            double result = tree.Evaluate();
            Assert.AreEqual(2, result);
        }

        [TestMethod()]
        public void EvaluateMultiplication()
        {
            Node root = new MultiplicationNode(new ConstantNode(5), new ConstantNode(3));
            ExpressionTree tree = new ExpressionTree(root);

            double result = tree.Evaluate();
            Assert.AreEqual(15, result);
        }

        [TestMethod()]
        public void EvaluateDivision()
        {
            Node root = new DivisionNode(new ConstantNode(6), new ConstantNode(3));
            ExpressionTree tree = new ExpressionTree(root);

            double result = tree.Evaluate();
            Assert.AreEqual(2, result);
        }

        [TestMethod()]
        public void BuildTreeTest1()
        {
            string infix = "1+3-2";
            ExpressionTree tree = new ExpressionTree(infix);
            
            Assert.AreEqual(2, tree.Evaluate());
        }

        [TestMethod()]
        public void BuildTreeTest2()
        {
            string infix = "1+(3-2)*2";
            ExpressionTree tree = new ExpressionTree(infix);

            Assert.AreEqual(3, tree.Evaluate());
        }

        [TestMethod()]
        public void BuildTreeTest3()
        {
            Dictionary<string, double> variables = new Dictionary<string, double>
            {
                { "B2", 0.0 }
            };
            string infix = "1+(3-2)*B2";
            ExpressionTree tree = new ExpressionTree(infix);

            Assert.AreEqual(1, tree.Evaluate());
        }
    }
}