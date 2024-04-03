using Microsoft.VisualStudio.TestTools.UnitTesting;
using spreadsheet2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spreadsheet2.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void InitializeDataGridTest()
        {
            DataGridView dgv = new DataGridView();

            Assert.IsTrue(dgv.ColumnCount == 26);
        }
    }
}