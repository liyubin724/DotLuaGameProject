namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.INT_NAME)]
    public class WDBIntField : WDBField
    {
        public override WDBFieldType FieldType
        {
            get
            {
                return WDBFieldType.Int;
            }
        }

        public WDBIntField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBCellValidationNames.INT_NAME };
        }
    }
}
