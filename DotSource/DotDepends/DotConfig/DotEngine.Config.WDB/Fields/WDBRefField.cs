namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.REF_NAME)]
    public class WDBRefField : WDBField
    {
        public override WDBFieldType FieldType
        {
            get
            {
                return WDBFieldType.Ref;
            }
        }

        public WDBRefField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "-1";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBCellValidationNames.INT_NAME };
        }
    }
}
