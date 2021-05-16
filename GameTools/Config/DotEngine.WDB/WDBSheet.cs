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

        public void Verify(StringContextContainer context)
        {
            List<string> errors = context.Get<List<string>>(WDBConst.CONTEXT_ERRORS_NAME);
            if(string.IsNullOrEmpty(Name))
            {
                errors.Add(WDBConst.VERIFY_SHEET_NAME_EMPTY_ERR);
            }else
            {
                if (!char.IsLetter(Name[0]))
                {
                    errors.Add(string.Format(WDBConst.VERIFY_SHEET_NAME_STARTWITH_ERROR, Name));
                }

                if(!Regex.IsMatch(Name,@"^[A-Za-z][A-Za-z0-9]{2,20}"))
                {

                }
            }



        }

        public string[] Verify()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new string[] { WDBConst.VERIFY_SHEET_NAME_EMPTY_ERR };
            }

            if (FieldCount == 0)
            {
                return new string[] { string.Format(WDBConst.VERIFY_SHEET_NO_FIELD_ERR, Name) };
            }
            if (RowCount == 0)
            {
                return new string[] { string.Format(WDBConst.VERIFY_SHEET_NO_ROW_ERR, Name) };
            }

            List<string> errors = new List<string>();
            foreach (var row in rows)
            {
                if (row.CellCount != FieldCount)
                {
                    errors.Add(string.Format(WDBConst.VERIFY_SHEET_FIELD_ROW_ERR, FieldCount, row.Row));
                }
            }
            if (errors.Count > 0)
            {
                return errors.ToArray();
            }

            foreach (var field in fields)
            {
                field.Verify(errors);
            }
            if (errors.Count > 0)
            {
                return errors.ToArray();
            }

            foreach (var row in rows)
            {
                for (int i = 0; i < FieldCount; ++i)
                {
                    WDBField field = fields[i];
                    WDBCell cell = row.GetCellByIndex(i);

                    if (field.Validations != null && field.Validations.Length > 0)
                    {
                        foreach (var validation in field.Validations)
                        {
                            string[] results = validation.Verify(this, field, cell);
                            if (results != null && results.Length > 0)
                            {
                                errors.AddRange(results);
                            }
                        }
                    }
                }
            }
            return errors.Count > 0 ? errors.ToArray() : null;
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
