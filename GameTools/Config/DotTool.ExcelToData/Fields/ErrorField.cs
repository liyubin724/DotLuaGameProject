using DotTool.ETD.Data;

namespace DotTool.ETD.Fields
{
    public class ErrorField : Field
    {
        public ErrorField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
        }

        public override object GetValue(Cell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}
