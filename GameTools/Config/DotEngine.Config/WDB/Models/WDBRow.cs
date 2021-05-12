using System.Collections.Generic;
using System.Text;

namespace DotEngine.Config.WDB
{
    public class WDBRow
    {
        private int row;
        private List<WDBCell> cells = new List<WDBCell>();

        public int Row => row;
        public int CellCount => cells.Count;

        public WDBRow(int row)
        {
            this.row = row;
        }

        public void AddCell(int col, string value)
        {
            WDBCell cell = new WDBCell(row, col, value);
            cells.Add(cell);
        }

        public WDBCell GetCellByCol(int col)
        {
            foreach (var cell in cells)
            {
                if (cell.Col == col)
                {
                    return cell;
                }
            }
            return null;
        }

        public WDBCell GetCellByIndex(int index)
        {
            if (index >= 0 && index < cells.Count)
            {
                return cells[index];
            }
            return null;
        }

        public WDBCell RemoveCellByCol(int c)
        {
            WDBCell cell = null;

            for (int i = 0; i < cells.Count; ++i)
            {
                cell = cells[i];
                if (cell.Col == c)
                {
                    cells.RemoveAt(i);
                    break;
                }
            }
            return cell;
        }

        public WDBCell RemoveCellByIndex(int index)
        {
            WDBCell cell = null;
            if (index >= 0 && index < cells.Count)
            {
                cell = cells[index];
                cells.RemoveAt(index);
                return cell;
            }
            return cell;
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
