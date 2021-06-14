namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Bool)]
    public class BoolField : WDBField
    {
        public BoolField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return "false";
        }
    }
}
