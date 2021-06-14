using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class BoolField : Field
    {
        public BoolField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            if(string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "false";
            }
        }

        protected override string GetDefaultValidation()
        {
            return "bool";
        }

        public override object GetValue(Cell cell)
        {
            string cellContent = cell.GetContent(this);
            return bool.Parse(cellContent);
        }
    }
}
