using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SpreadSheetEngine
{
    //container for a 2D array of cells
    public class Spreadsheet
    {
        //holds the cells of the spreadsheet
        private SpreadsheetCell[,] _spreadsheetCells;

        //for circular reference
        public Dictionary<string, List<string>> cirRef = new();
        public string CurrentRef; 

        public event PropertyChangedEventHandler? OnCellPropertyChanged;

        private XDocument? doc { get; set; }

        public int ColumnCount
        {
            get
            {
                return _spreadsheetCells.GetLength(0);
            }
        }

        public int RowCount
        {
            get
            {
                return _spreadsheetCells.GetLength(1);
            }
        }
        public Spreadsheet(int columns, int rows)
        {
            this._spreadsheetCells = new SpreadsheetCell[columns, rows];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    this._spreadsheetCells[column, row] = new SpreadsheetCell(column, row, this);
                    this._spreadsheetCells[column, row].PropertyChanged += this.CellPropertyChanged;
                }
            }

        }



        /// <summary>
        /// Notifies when a cell property has changed
        /// </summary>
        /// <param name="sender"> Object theat will be routed</param>
        /// <param name="arg">the changed events arguments</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs arg)
        {
            if (sender is SpreadsheetCell c)
            {
                switch(arg.PropertyName)
                {
                    case nameof(c.Text):
                        this.OnCellPropertyChanged?.Invoke(sender, arg);
                        break;
                    case nameof(c.BGColor):
                        this.OnCellPropertyChanged?.Invoke(sender, arg);
                        break;
                    case nameof(c.Value):
                        this.OnCellPropertyChanged?.Invoke(sender, arg);
                        break;
                    default:
                        throw new NotImplementedException($"Error with {arg.PropertyName} ");
                }
            }
        }

        public Cell getCell(int columnIndex, int rowIndex)
        {
            return this._spreadsheetCells[columnIndex, rowIndex];
        }

        public Cell getCell(string index)
        {
            int columnIndex = index[0] - 65;
            int rowIndex = index[1] - 49;
            return this._spreadsheetCells[columnIndex, rowIndex];
        }


        public void setText(int columnIndex, int rowIndex, string txt)
        {
            this._spreadsheetCells[columnIndex, rowIndex].Text = txt;
        }

        public void setValue(int columnIndex, int rowIndex, string value)
        {
            this._spreadsheetCells[columnIndex, rowIndex].Text = value;
        }

        public void Demo()
        {
            Random random = new();

            for (int i =0; i < 50; i++)
            {
                int rColumn = random.Next(0, 25);
                int rRow = random.Next(0, 49);

                SpreadsheetCell sheetCell = this._spreadsheetCells[rColumn, rRow];
                sheetCell!.Text = "Hello !";
                this._spreadsheetCells[rColumn, rRow] = sheetCell;
            }

            for (int i = 0; i <50; i++)
            {
                this._spreadsheetCells[1, i].Text = $"This is cell B{i}";
            }
        }

        public void SetBGColor(IEnumerable<Cell> cells, uint color)
        {
            foreach(Cell cell in cells)
            {
                cell.BGColor = color;
            }
        }

        public void SaveSpreadSheet(System.IO.Stream saveStream)
        {
            this.doc = new XDocument();
            var xmlfile = new XElement(nameof(Spreadsheet));

            foreach (SpreadsheetCell sCell in this._spreadsheetCells)
            {
                if (sCell.Text != string.Empty || sCell.BGColor != 0xFFFFFFFF)
                {
                    xmlfile.Add(new XElement(
                        nameof(SpreadsheetCell),
                        new XElement(nameof(Cell.BGColor), sCell.BGColor.ToString()),
                        new XElement(nameof(Cell.Text), sCell.Text.ToString()),
                        new XAttribute(nameof(Cell.Index), sCell.Index)
                        ));
                }
            }

            this.doc.Add(xmlfile);
            this.doc.Save(saveStream);
        }

        public void LoadSpreadSheet(System.IO.Stream loadStream)
        {
            int columns = this.ColumnCount;
            int rows = this.RowCount;
            this._spreadsheetCells = new SpreadsheetCell[columns, rows];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    this._spreadsheetCells[column, row] = new SpreadsheetCell(column, row, this);
                    this._spreadsheetCells[column, row].PropertyChanged += this.CellPropertyChanged;
                }
            }

            XDocument loadDoc = XDocument.Load(loadStream);

            this.doc = loadDoc;
            if (this.doc.Root is null)
            {
                return;
            }

            

            switch (this.doc.Root.Name.ToString())
            {
                case nameof(Spreadsheet):
                    {
                        IEnumerable<XElement> spreadSheetCells = this.doc.Root.Elements(nameof(SpreadsheetCell));
                        foreach (XElement cell in spreadSheetCells)
                        {
                            string cellName = cell.FirstAttribute.Value;
                            int colIndex = (int)cellName[0] - 65;
                            int rowIndex = int.Parse(cellName.Substring(1))-1;
                            uint color = uint.Parse(cell.Element(nameof(Cell.BGColor)).Value);
                            string cellText = cell.Element(nameof(Cell.Text)).Value;

                            if (color is { })
                            {
                                this._spreadsheetCells[colIndex,rowIndex].BGColor = color;
                            }

                            if (cellText is { })
                            {
                                this._spreadsheetCells[colIndex, rowIndex].Text = cellText;
                            }

                        }
                    }
                    break;
            }
            return;
        }

    }
}
