namespace DotEngine.Config.WDB
{
    [CustomField("bool")]
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
