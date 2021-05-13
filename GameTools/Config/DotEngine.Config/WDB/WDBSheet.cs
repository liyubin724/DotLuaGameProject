using System.Collections.Generic;
using System.Text;

namespace DotEngine.WDB
{
    public class WDBSheet
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

        public WDBField AddField(int col,string name,string desc,string type,string platform,string defaultValue,string validationRule)
        {
            WDBField field = WDBUtility.CreateField(col, name, desc, type, platform, defaultValue, validationRule);
            fields.Add(field);

            return field;
        }

        public WDBField GetFieldAtCol(int col)
        {
            foreach(var field in fields)
            {
                if(field.Col == col)
                {
                    return field;
                }
            }
            return null;
        }

        public WDBField GetFieldAtIndex(int index)
        {
            if(index>=0&&index <fields.Count)
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
            foreach(var r in rows)
            {
                if(r.Row == row)
                {
                    return r;
                }
            }
            return null;
        }

        public WDBRow GetRowAtIndex(int index)
        {
            if(index>=0 && index < rows.Count)
            {
                return rows[index];
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{{name = {Name},fieldcount = {FieldCount},rowcount = {RowCount},");
            builder.AppendLine("fields = {");
            foreach(var field in fields)
            {
                builder.AppendLine($"\t{field},");
            }
            builder.AppendLine("}");
            builder.AppendLine("rows = {");
            foreach(var row in rows)
            {
                builder.AppendLine("$\t{row}");
            }
            builder.AppendLine("}");
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
