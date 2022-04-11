namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.LONG_NAME)]
    public class WDBLongField : WDBField
    {
        public WDBLongField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBValidationNames.LONG_NAME };
        }
    }
}
