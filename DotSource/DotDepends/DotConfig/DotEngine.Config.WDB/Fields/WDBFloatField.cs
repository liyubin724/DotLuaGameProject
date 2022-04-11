namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.FLOAT_NAME)]
    public class WDBFloatField : WDBField
    {
        public WDBFloatField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "0.0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBValidationNames.FLOAT_NAME };
        }
    }
}
