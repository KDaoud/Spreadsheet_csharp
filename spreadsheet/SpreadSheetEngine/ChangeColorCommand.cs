using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class ChangeColorCommand : ICommand
    {
        private readonly Spreadsheet _spreadsheet;
        private uint _color;
        private uint _previousColor;
        private IEnumerable<Cell> _cells;
        

        public ChangeColorCommand(Spreadsheet spreadsheet, IEnumerable<Cell> cells, uint color)
        {
            _spreadsheet = spreadsheet;
            _color = color;
            _cells = cells;
            _previousColor = cells.ToArray()[0].BGColor;
        }

        public void Execute()
        {
            _spreadsheet.SetBGColor(_cells, _color);
        }

        public void Redo()
        {
            _spreadsheet.SetBGColor(_cells, _color);
        }

        public void Undo()
        {
            //current undo just return cell to white
            _spreadsheet.SetBGColor(_cells, _previousColor);
        }
    }
}
