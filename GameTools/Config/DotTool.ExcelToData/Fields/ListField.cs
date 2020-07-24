using DotTool.ETD.Data;
using System;

namespace DotTool.ETD.Fields
{
    public class ListField : Field
    {
        public FieldType ValueType { get; private set; } = FieldType.None;
        public string ValueRefName { get; private set; } = string.Empty;

        public ListField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            ValueType = FieldTypeUtil.GetListInnerType(t, out string refName);
            ValueRefName = refName;
        }

        protected override string GetDefaultValidation()
        {
            return "list";
        }

        public override object GetValue(Cell cell)
        {
            string cellContent = cell.GetContent(this);
            if(string.IsNullOrEmpty(cellContent))
            {
                return null;
            }else
            {
                Type valueRealyType = FieldTypeUtil.GetRealyType(ValueType);

                cellContent = cellContent.Substring(1, cellContent.Length - 2);
                string[] values = cellContent.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if(values!=null && values.Length>0)
                {
                    Array expanded = Array.CreateInstance(valueRealyType, values.Length);
                    for(int i =0;i<values.Length;++i)
                    {
                        object valueObj = null;
                        string valueStr = values[i];
                        if(valueRealyType == typeof(string))
                        {
                            valueObj = valueStr;
                        }else if(valueRealyType == typeof(int))
                        {
                            valueObj = int.Parse(valueStr);
                        }else if(valueRealyType == typeof(long))
                        {
                            valueObj = long.Parse(valueStr);
                        }else if(valueRealyType == typeof(float))
                        {
                            valueObj = float.Parse(valueStr);
                        }else if(valueRealyType == typeof(bool))
                        {
                            valueObj = bool.Parse(valueStr);
                        }

                        expanded.SetValue(valueObj, i);
                    }
                    return expanded;
                }
                return null;
            }
        }
    }
}
