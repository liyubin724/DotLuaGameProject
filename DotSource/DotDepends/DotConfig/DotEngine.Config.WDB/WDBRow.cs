﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotEngine.Config.WDB
{
    public class  WDBRow : IEnumerable<WDBCell>,IWDBValidationChecker
    {
        public int Row { get; private set; }

        private List<WDBCell> cells = new List<WDBCell>();

        public int CellCount => cells.Count;

        public WDBRow(int row)
        {
            Row = row;
        }

        public WDBCell AddCell(int column, string content)
        {
            WDBCell cell = new WDBCell(Row, column);
            cell.Content = content;
            cells.Add(cell);
            return cell;
        }

        public WDBCell GetCellByIndex(int index)
        {
            if(index< 0 || index>= cells.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return cells[index];
        }

        public WDBCell GetCellByColumn(int column)
        {
            foreach(var cell in cells)
            {
                if(cell.Column == column)
                {
                    return cell;
                }
            }
            return null;
        }

        public WDBCell RemoveCellAtIndex(int index)
        {
            if(index<0 || index>= cells.Count)
            {
                throw new IndexOutOfRangeException();
            }

            WDBCell cell = cells[index];
            cells.RemoveAt(index);

            return cell;
        }

        public WDBCell RemoveCellAtColumn(int column)
        {
            for (int i = 0; i < cells.Count; ++i)
            {
                if (cells[i].Column == column)
                {
                    WDBCell cell = cells[i];
                    cells.RemoveAt(i);
                    return cell;
                }
            }

            return null;
        }

        public IEnumerator<WDBCell> GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < cells.Count; ++i)
            {
                yield return cells[i];
            }
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("{");
            foreach(var cell in cells)
            {
                strBuilder.Append(cell.ToString());
                strBuilder.Append(",");
            }
            strBuilder.Append("}");
            return strBuilder.ToString();
        }

        public void Check(WDBContext context)
        {
            WDBSheet sheet = context.Get<WDBSheet>(WDBContextKey.CURRENT_SHEET_NAME);
            context.Add(WDBContextKey.CURRENT_ROW_NAME, this);
            {
                if(sheet.FieldCount!=CellCount)
                {
                    context.AppendError(string.Format(WDBErrorMessages.ROW_CELL_COUNT_ERROR, CellCount, Row, sheet.FieldCount));
                }
                else
                {
                    for(int i =0;i<CellCount;i++)
                    {
                        context.Add(WDBContextKey.CURRENT_FIELD_NAME, sheet.GetFieldAtIndex(i));
                        {
                            cells[i].Check(context);
                        }
                        context.Remove(WDBContextKey.CURRENT_FIELD_NAME);
                    }
                }
            }
            context.Remove(WDBContextKey.CURRENT_ROW_NAME);
        }
    }
}
