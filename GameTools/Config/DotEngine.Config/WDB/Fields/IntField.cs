namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Int)]
    public class IntField : WDBField
    {
        public IntField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return "0";
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "int" };
        }
    }
}
