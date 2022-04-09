using System;
using System.Collections.Generic;
using System.Text;

namespace DotEngine.Config.WDB
{
    public class WDBSheet
    {
        public string Name { get; private set; }

        private List<WDBField> fields = new List<WDBField>();
        private List<WDBRow> rows = new List<WDBRow>();

        public int FieldCount => fields.Count;
        public int RowCount => rows.Count;

        public WDBSheet(string name)
        {
            Name = name;
        }

        public WDBField AddField(
            int col,
            string name,
            string desc,
            string type,
            string platform,
            string defaultContent,
            string validation)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new WDBFieldTypeEmptyException(col, name);
            }
            WDBField field = WDBFieldFactory.CreateField(col, type);
            field.Name = name;
            field.Desc = desc;
            field.Platform = platform;
            field.DefaultContent = defaultContent;
            field.Validation = validation;

            fields.Add(field);

            return field;
        }

        public void AddFields(WDBField[] fields)
        {
            this.fields.AddRange(fields);
        }

        public WDBField GetFieldAtIndex(int index)
        {
            if (index < 0 || index >= fields.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return fields[index];
        }

        public WDBField GetFieldAtColumn(int column)
        {
            foreach (var field in fields)
            {
                if (field.Column == column)
                {
                    return field;
                }
            }
            return null;
        }

        public WDBRow CreateRow(int row)
        {
            WDBRow r = new WDBRow(row);
            rows.Add(r);
            return r;
        }

        public void AddRows(WDBRow[] rows)
        {
            this.rows.AddRange(rows);
        }

        public WDBRow GetRowAtIndex(int index)
        {
            if (index < 0 || index >= rows.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return rows[index];
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine($"name = {Name},fieldcount = {FieldCount},rowcount = {RowCount},");
            builder.AppendLine("fields = {");
            foreach (var field in fields)
            {
                builder.AppendLine($"\t{field},");
            }
            builder.AppendLine("}");
            builder.AppendLine("rows = {");
            foreach (var row in rows)
            {
                builder.AppendLine($"\t{row}");
            }
            builder.AppendLine("}");
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
