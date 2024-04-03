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
    public class parseTests
    {
        [TestMethod()]
        public void parsingTest()
        {
            string input = "H1 + 3 * Hello / 2";
            parse p = new parse(input);
            string[] result = p.parsing();
            string[] expected = { "H1", "+", "3", "*", "Hello", "/", "2" };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}