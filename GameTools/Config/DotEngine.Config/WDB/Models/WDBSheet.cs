using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
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
            WDBField field = new WDBField()
            {
                Col = col,
                Name = name,
                Desc = desc,
                Type = type,
                Platform = platform,
                DefaultValue = defaultValue,
                ValidationRule = validationRule,
            };

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

    }
}
