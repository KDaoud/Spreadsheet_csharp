using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine 
{
    /// <summary>
    /// Represents a Single Cell in the Spreadsheet
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        //in cell text
        protected string inText;

        //in cell value
        protected string inValue;

        //Error message
        protected string inError;


        //cell background color
        protected uint inColor { get; set; }
        //row index, read only
        public int RowIndex { get; }

        //column index, read only
        public int ColumnIndex { get; }

        //Event handler to hold the propety changed of a cell
        public event PropertyChangedEventHandler? PropertyChanged;

        //Text  Property 
        public string Text
        {
            get { return this.inText; }

            set 
            {
                this.inText = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Text)));
            }
        }

        //in Value
        public string Value
        {
            get { return this.inValue; }
            set 
            {
                if (this.inValue != value)
                {
                    this.inValue = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
                }
            }
        }

        //Value property
        public string CellValue
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.inError))
                {
                    return this.inError ?? throw new InvalidOperationException();
                }
                else
                {
                    return this.inValue ?? string.Empty;
                }
            }
        }

        public void SetCellValue(string value)
        {
            this.inValue = value;
        }


        //Color property
        public uint BGColor
        {
            get => this.inColor;
            set
            {
                if (this.inColor == value)
                {
                    return;
                }

                this.inColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.BGColor)));
            }
        }

        protected Cell(int columnIndex,int rowIndex)
        {
            this.ColumnIndex = columnIndex;
            this.RowIndex = rowIndex;
            this.Text = string.Empty;
            this.BGColor = 0xFFFFFFFF;
            this.inError = "#ERROR";
        }

        public string Index
        {
            get
            {
                return (char)(this.ColumnIndex + 65) + (this.RowIndex + 1).ToString();
            }
        }

        public void Update()
        {
            this.SetCellValue(this.CellValue);
        }

    }
}
