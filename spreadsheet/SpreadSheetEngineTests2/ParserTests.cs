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
    public class ParserTests
    {
        [TestMethod()]
        public void ConvertToRPNTest()
        {
            string expression = "2 + H1 * 4";
            List<string> expected = new List<string>() { "2", "H1", "4", "*", "+" };
            Parser parser = new Parser();
            List<string> results = parser.ConvertToRPN(expression);
            CollectionAssert.AreEqual(expected, results);
        }
    }
}