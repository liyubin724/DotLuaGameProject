namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.DATETIME_NAME)]
    public class WDBDateTimeField : WDBField
    {
        public WDBDateTimeField(int column, string type) : base(column, type)
        {
        }

        public override WDBFieldType FieldType
        {
            get
            {
                return WDBFieldType.DateTime;
            }
        }

        protected override string GetTypeDefaultContent()
        {
            return "1970-1-1 0:0:0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBCellValidationNames.DATETIME_NAME };
        }
    }
}
