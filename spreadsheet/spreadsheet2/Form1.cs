using SpreadSheetEngine;
using System.ComponentModel;
using System.Drawing.Configuration;
using System.Windows.Forms;

namespace spreadsheet2
{
    public partial class Form1 : Form
    {
        private Spreadsheet _spreadsheet { get; }
        CommandHistory commandHistory = new CommandHistory();

        public Form1()
        {
            this._spreadsheet = new SpreadSheetEngine.Spreadsheet(26, 50);
            this._spreadsheet.OnCellPropertyChanged += this.ChangeCell;
            InitializeComponent();
            InitializeDataGrid();

        }

        /// <summary>
        /// Initialize The DataGrid
        /// </summary>
        public void InitializeDataGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 26;
            int index = 0;

            for (char c = 'A'; c <= 'Z'; c++)
            {
                dataGridView1.Columns[index].Name = c.ToString();
                index++;
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(50);
            index = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                index++;
                row.HeaderCell.Value = index.ToString();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._spreadsheet.Demo();
        }

        private void ChangeCell(object sender, PropertyChangedEventArgs arg)
        {
            if (sender is Cell c)
            {
                switch (arg.PropertyName)
                {
                    case nameof(Cell.Text):
                        this.dataGridView1.Rows[c.RowIndex].Cells[c.ColumnIndex].Value = c.Value;
                        break;
                    case nameof(Cell.BGColor):
                        this.dataGridView1.Rows[c.RowIndex].Cells[c.ColumnIndex].Style.BackColor = System.Drawing.Color.FromArgb((int)c.BGColor);
                        break;
                    case nameof(Cell.Value):
                        break;
                    default:
                        break;
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string? currText = this._spreadsheet.getCell(e.ColumnIndex, e.RowIndex).Text;

            if (currText != null)
            {
                this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = currText;
            }
        }

        private void dataGridView1_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            string endText;

            var currValue = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;


            if (currValue != null)
            {
                endText = currValue.ToString();
                Cell c = _spreadsheet.getCell(e.ColumnIndex,e.RowIndex);
                var command = new ChangeTextCommand(this._spreadsheet, c, endText);

                commandHistory.Execute(command);
                //this._spreadsheet.setText(e.ColumnIndex, e.RowIndex, endText);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowHelp = true;

            uint chosenColor;


            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                chosenColor = (uint)colorDialog.Color.ToArgb();
                IEnumerable<Cell> cells = this.dataGridView1.SelectedCells.Cast<DataGridViewCell>().Select(item => this._spreadsheet.getCell(item.ColumnIndex, item.RowIndex));
                //this._spreadsheet.SetBGColor(cells, chosenColor);
                var command = new ChangeColorCommand(this._spreadsheet, cells,chosenColor);
                
                commandHistory.Execute(command);
            }


        }

        //save xml
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "xml | *.xml";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.ValidateNames = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this._spreadsheet.SaveSpreadSheet(saveFileDialog.OpenFile());
            }
        }

        //load xml
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "xml | *.xml";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = false;

            

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.InitializeDataGrid();
                commandHistory.Clear();
                this._spreadsheet.LoadSpreadSheet(openFileDialog.OpenFile());
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            commandHistory.Undo();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            commandHistory.Redo();
        }
    }
}