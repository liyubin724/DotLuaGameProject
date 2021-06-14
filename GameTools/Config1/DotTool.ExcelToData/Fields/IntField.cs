using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class IntField : Field
    {
        public IntField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            if(string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "0";
            }
        }

        protected override string GetDefaultValidation()
        {
            return "int";
        }

        public override object GetValue(Cell cell)
        {
            string cellContent = cell.GetContent(this);
            return int.Parse(cellContent);
        }
    }
}
