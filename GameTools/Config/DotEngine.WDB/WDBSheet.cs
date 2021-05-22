using DotEngine.Context;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DotEngine.WDB
{
    public class WDBSheet : IVerify
    {
        public string Name { get; private set; }

        private List<WDBField> fields = new List<WDBField>();
        private List<WDBRow> rows = new List<WDBRow>();

        public int FieldCount => fields.Count;
        public int RowCount => rows.Count;

        public WDBSheet(string n)
        {
            Name = n;
        }

        public WDBField AddField(int col, string name, string desc, string type, string platform, string defaultValue, string validationRule)
        {
            WDBField field = WDBUtility.CreateField(col, name, desc, type, platform, defaultValue, validationRule);
            fields.Add(field);

            return field;
        }

        public WDBField GetFieldAtCol(int col)
        {
            foreach (var field in fields)
            {
                if (field.Col == col)
                {
                    return field;
                }
            }
            return null;
        }

        public WDBField GetFieldAtIndex(int index)
        {
            if (index >= 0 && index < fields.Count)
            {
                return fields[index];
            }
            return null;
        }

        public WDBRow AddRow(int row)
        {
            WDBRow r = new WDBRow(row);
            rows.Add(r);
            return r;
        }

        public WDBRow GetRowAtRow(int row)
        {
            foreach (var r in rows)
            {
                if (r.Row == row)
                {
                    return r;
                }
            }
            return null;
        }

        public WDBRow GetRowAtIndex(int index)
        {
            if (index >= 0 && index < rows.Count)
            {
                return rows[index];
            }
            return null;
        }

        public bool Verify(ref List<string> errors)
        {
            if(string.IsNullOrEmpty(Name))
            {
                errors.Add(WDBConst.VERIFY_SHEET_NAME_EMPTY_ERR);
                return false;
            }
            if (!Regex.IsMatch(Name, WDBConst.VERIFY_SHEET_NAME_REGEX))
            {
                errors.Add(string.Format(WDBConst.VERIFY_SHEET_NAME_REGEX_ERR, Name));
                return false;
            }

            if(FieldCount == 0)
            {
                errors.Add(string.Format(WDBConst.VERIFY_SHEET_NO_FIELD_ERR, Name));
                return false;
             }
            if(RowCount == 0)
            {
                errors.Add(string.Format(WDBConst.VERIFY_SHEET_NO_ROW_ERR, Name));
                return false;
            }

            bool result = true;
            foreach(var field in fields)
            {
                if(!field.Verify(ref errors))
                {
                    if (result) result = false;
                }
            }
            if(!result)
            {
                return false;
            }

            StringContextContainer context = new StringContextContainer();
            context.Add(WDBConst.CONTEXT_SHEET_NAME, this);
            context.Add(WDBConst.CONTEXT_ERRORS_NAME, errors);
            foreach (var row in rows)
            {
                if (row.CellCount != FieldCount)
                {
                    if (result) result = false;
                    errors.Add(string.Format(WDBConst.VERIFY_SHEET_FIELD_ROW_ERR, FieldCount, row.Row));
                }
            }

            if(!result)
            {
                return false;
            }

            for(int i =0;i<fields.Count;++i)
            {
                var field = fields[i];
                context.Add(WDBConst.CONTEXT_FIELD_NAME, field);
                foreach(var row in rows)
                {
                    var cell = row.GetCellByIndex(i);
                    if(cell.Col != field.Col)
                    {
                        if (result) result = false;
                        errors.Add(string.Format(WDBConst.VERIFY_CELL_COL_NOTSAME_ERR, cell.Row, cell.Col));
                    }else
                    {
                        context.Add(WDBConst.CONTEXT_CELL_NAME, cell);
                        foreach(var cellValidation in field.Validations)
                        {
                            context.InjectTo(cellValidation);
                        }
                        context.Remove(WDBConst.CONTEXT_CELL_NAME);
                    }
                }
                context.Remove(WDBConst.CONTEXT_FIELD_NAME);
            }
            return errors.Count == 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{{name = {Name},fieldcount = {FieldCount},rowcount = {RowCount},");
            builder.AppendLine("fields = {");
            foreach (var field in fields)
            {
                builder.AppendLine($"\t{field},");
            }
            builder.AppendLine("}");
            builder.AppendLine("rows = {");
            foreach (var row in rows)
            {
                builder.AppendLine("$\t{row}");
            }
            builder.AppendLine("}");
            builder.AppendLine("}");
            return builder.ToString();
        }


    }
}
