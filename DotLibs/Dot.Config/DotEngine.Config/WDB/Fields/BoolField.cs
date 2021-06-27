namespace DotEngine.Config.WDB
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

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "bool" };
        }
    }
}
