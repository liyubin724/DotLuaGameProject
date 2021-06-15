namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Long)]
    public class LongField : WDBField
    {
        public LongField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return "0";
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "long" };
        }
    }
}
