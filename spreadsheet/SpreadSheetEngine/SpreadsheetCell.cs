using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    //conrete cell from abstract class
    public class SpreadsheetCell : Cell
    {
        private Spreadsheet _spreadSheet{ get; set; }

        public SpreadsheetCell(int columnIndex, int rowIndex, Spreadsheet spreadSheet)
            : base (columnIndex, rowIndex)
        {
            this.PropertyChanged += this.CellPropertyChanged;
            this._spreadSheet = spreadSheet;
        }

        private void CellPropertyChanged(object sender, PropertyChangedEventArgs arg)
        {
            if (sender is SpreadsheetCell c)
            {
                switch (arg.PropertyName)
                {
                    case nameof(c.Text):
                    case nameof(c.Value):
                        this.UpdateValue();
                        break;
                    case nameof(c.BGColor):
                        break;
                    default:
                        break;
                }
            }
        }

        public bool checkCircular(string index)
        {
            try
            {
                foreach (string item in _spreadSheet.cirRef[index])
                {
                    if (item == _spreadSheet.CurrentRef)
                    {
                        return true;
                    }
                }
            }catch //key not found
            {
                return false;
            }
            
            return false;
        }

        public void UpdateValue()
        {
            if (this.Text.StartsWith("=") && this.Text.Length > 1)
            {
                try
                {
                    //
                    IEnumerable<Cell?> referencedCells;
                    List<string> circularRef = new List<string>();
                    ExpressionTree expTree = new ExpressionTree(this.Text.Substring(1));
                    Cell mycell = this._spreadSheet.getCell("B1");
                    referencedCells = expTree.Variables.Select(item => this._spreadSheet.getCell(item.Key));

                    foreach(Cell refCell in referencedCells)
                    {
                        if (refCell.Index == this.Index)
                        {
                            this.SetCellValue("#ERROR");
                            return;
                        }
                        circularRef.Add(refCell.Index);
                    }

                    //this._spreadSheet.cirRef.Remove(this.Index);
                    this._spreadSheet.cirRef.Add(this.Index, circularRef);
                    this._spreadSheet.CurrentRef = this.Index;

                    foreach (string item in _spreadSheet.cirRef[Index])
                    {
                        if(checkCircular(item))
                        {
                            this.SetCellValue("#ERROR");
                            return;
                        }
                    }
                   


                    foreach (KeyValuePair<string, double> kv in expTree.Variables)
                    {
                        try 
                        {
                            if(double.TryParse(this._spreadSheet.getCell(kv.Key).Value, out double cValue))
                            {
                                expTree.SetVariable(kv.Key, cValue);
                            }
                            else
                            {
                                expTree.SetVariable(kv.Key, 0.0);
                            }
                        }catch
                        {
                            this.SetCellValue("#ERROR");
                        }
                    }



                    string output = expTree.Evaluate().ToString();
                    this.SetCellValue(output);

                    foreach(SpreadsheetCell c in referencedCells)
                    {
                        c.UpdateValue();

                    }

                }
                catch (Exception e)
                {
                    //

                }
            }
            else
            {
                try
                {
                    this.SetCellValue(this.Text);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }


            
        }
    }
}
