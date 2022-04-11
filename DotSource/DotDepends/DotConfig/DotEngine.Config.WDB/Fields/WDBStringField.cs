namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.STRING_NAME)]
    public class WDBStringField : WDBField
    {
        public WDBStringField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return string.Empty;
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return null;
        }
    }
}
