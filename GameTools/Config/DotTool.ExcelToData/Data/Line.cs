using System.Collections.Generic;
using System.Text;

namespace DotTool.ETD.Data
{
    public class Line
    {
        private int row;
        private List<Cell> cells = new List<Cell>();

        public int Row { get => row; }
        public int Count { get => cells.Count; }

        public Line(int r)
        {
            row = r;
        }

        public void AddCell(int c,string value)
        {
            Cell cell = new Cell(row, c, value);
            cells.Add(cell);
        }

        public Cell GetCellByCol(int col)
        {
            foreach(var cell in cells)
            {
                if(cell.Col == col)
                {
                    return cell;
                }
            }
            return null;
        }

        public Cell GetCellByIndex(int index)
        {
            if(index>=0&&index<cells.Count)
            {
                return cells[index];
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var cell in cells)
            {
                sb.Append(cell.ToString() + ",");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
