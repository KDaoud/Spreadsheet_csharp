using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class ChangeTextCommand : ICommand
    {
        private readonly Spreadsheet _spreadsheet;
        private string _newText;
        private string _previousText;
        private Cell _cell;
        

        public ChangeTextCommand(Spreadsheet spreadsheet, Cell cell, string newText)
        {
            _spreadsheet = spreadsheet;
            _newText = newText;
            _cell = cell;
            _previousText = _spreadsheet.getCell(_cell.ColumnIndex, _cell.RowIndex).Text;
        }

        public void Execute()
        {
            _spreadsheet.setText(_cell.ColumnIndex, _cell.RowIndex, _newText);
            //_spreadsheet.setValue(_cell.ColumnIndex, _cell.RowIndex, _newText);
        }

        public void Redo()
        {
            _spreadsheet.setText(_cell.ColumnIndex, _cell.RowIndex, _newText);
        }

        public void Undo()
        {
            //current undo just return cell to white
            _spreadsheet.setText(_cell.ColumnIndex, _cell.RowIndex, _previousText);
            //_spreadsheet.setValue(_cell.ColumnIndex, _cell.RowIndex, _previousValue);
        }
    }
}
