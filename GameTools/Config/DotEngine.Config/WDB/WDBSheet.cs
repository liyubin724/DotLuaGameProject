using System.Collections.Generic;
using System.Text;

namespace DotEngine.Config.WDB
{
    public class WDBSheet
    {
        public string Name { get; private set; }

        private List<WDBField> fields = new List<WDBField>();
        private List<WDBLine> lines = new List<WDBLine>();

        public int FieldCount => fields.Count;
        public int RowCount => lines.Count;

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

        public WDBLine AddLine(int row)
        {
            WDBLine r = new WDBLine(row);
            lines.Add(r);
            return r;
        }

        public WDBLine GetLineAtRow(int row)
        {
            foreach (var r in lines)
            {
                if (r.Row == row)
                {
                    return r;
                }
            }
            return null;
        }

        public WDBLine GetLineAtIndex(int index)
        {
            if (index >= 0 && index < lines.Count)
            {
                return lines[index];
            }
            return null;
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
            foreach (var row in lines)
            {
                builder.AppendLine("$\t{row}");
            }
            builder.AppendLine("}");
            builder.AppendLine("}");
            return builder.ToString();
        }

        //public bool Verify(ref List<string> errors)
        //{
        //    if(string.IsNullOrEmpty(Name))
        //    {
        //        errors.Add(WDBConst.VERIFY_SHEET_NAME_EMPTY_ERR);
        //        return false;
        //    }
        //    if (!Regex.IsMatch(Name, WDBConst.VERIFY_SHEET_NAME_REGEX))
        //    {
        //        errors.Add(string.Format(WDBConst.VERIFY_SHEET_NAME_REGEX_ERR, Name));
        //        return false;
        //    }

        //    if(FieldCount == 0)
        //    {
        //        errors.Add(string.Format(WDBConst.VERIFY_SHEET_NO_FIELD_ERR, Name));
        //        return false;
        //     }
        //    if(RowCount == 0)
        //    {
        //        errors.Add(string.Format(WDBConst.VERIFY_SHEET_NO_ROW_ERR, Name));
        //        return false;
        //    }

        //    bool result = true;
        //    foreach(var field in fields)
        //    {
        //        if(!field.Verify(ref errors))
        //        {
        //            if (result) result = false;
        //        }
        //    }
        //    if(!result)
        //    {
        //        return false;
        //    }

        //    StringContextContainer context = new StringContextContainer();
        //    context.Add(WDBConst.CONTEXT_SHEET_NAME, this);
        //    context.Add(WDBConst.CONTEXT_ERRORS_NAME, errors);
        //    foreach (var row in lines)
        //    {
        //        if (row.CellCount != FieldCount)
        //        {
        //            if (result) result = false;
        //            errors.Add(string.Format(WDBConst.VERIFY_SHEET_FIELD_ROW_ERR, FieldCount, row.Row));
        //        }
        //    }

        //    if(!result)
        //    {
        //        return false;
        //    }

        //    for(int i =0;i<fields.Count;++i)
        //    {
        //        var field = fields[i];
        //        context.Add(WDBConst.CONTEXT_FIELD_NAME, field);
        //        foreach(var row in lines)
        //        {
        //            var cell = row.GetCellByIndex(i);
        //            if(cell.Col != field.Col)
        //            {
        //                if (result) result = false;
        //                errors.Add(string.Format(WDBConst.VERIFY_CELL_COL_NOTSAME_ERR, cell.Row, cell.Col));
        //            }else
        //            {
        //                context.Add(WDBConst.CONTEXT_CELL_NAME, cell);
        //                foreach(var cellValidation in field.Validations)
        //                {
        //                    context.InjectTo(cellValidation);
        //                }
        //                context.Remove(WDBConst.CONTEXT_CELL_NAME);
        //            }
        //        }
        //        context.Remove(WDBConst.CONTEXT_FIELD_NAME);
        //    }
        //    return errors.Count == 0;
        //}
    }
}
