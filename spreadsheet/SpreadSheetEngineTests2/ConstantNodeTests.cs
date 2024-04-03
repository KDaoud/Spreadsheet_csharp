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
    public class ConstantNodeTests
    {
        [TestMethod()]
        public void EvaluateTest()
        {
            Node root = new ConstantNode(5);
            ExpressionTree tree = new ExpressionTree(root);
            double result = tree.Evaluate();
            Assert.AreEqual(5, result);
        }
    }
}