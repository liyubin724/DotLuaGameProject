namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.BOOL_NAME)]
    public class WDBBoolField : WDBField
    {
        public WDBBoolField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "false";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBValidationNames.BOOL_NAME };
        }
    }
}
