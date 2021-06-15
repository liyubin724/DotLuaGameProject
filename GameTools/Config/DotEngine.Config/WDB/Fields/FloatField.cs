namespace DotEngine.Config.WDB
{

    [WDBFieldLink(WDBFieldType.Float)]
    public class FloatField : WDBField
    {
        public FloatField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return "0.0";
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "float" };
        }
    }
}
