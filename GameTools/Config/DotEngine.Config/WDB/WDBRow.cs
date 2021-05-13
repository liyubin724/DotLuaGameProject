﻿using System.Collections.Generic;
using System.Text;

namespace DotEngine.WDB
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
            for (int i = 0; i < cells.Count; ++i)
            {
                if (cells[i].Col == c)
                {
                    WDBCell cell = cells[i];
                    cells.RemoveAt(i);
                    return cell;
                }
            }
            return null;
        }

        public WDBCell RemoveCellByIndex(int index)
        {
            if (index >= 0 && index < cells.Count)
            {
                WDBCell cell = cells[index];
                cells.RemoveAt(index);
                return cell;
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
