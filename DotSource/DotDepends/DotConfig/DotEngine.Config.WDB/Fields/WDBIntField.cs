namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.INT_NAME)]
    public class WDBIntField : WDBField
    {
        public WDBIntField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBValidationNames.INT_NAME };
        }
    }
}
