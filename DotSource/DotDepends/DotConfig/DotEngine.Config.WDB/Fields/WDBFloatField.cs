namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.FLOAT_NAME)]
    public class WDBFloatField : WDBField
    {
        public override WDBFieldType FieldType
        {
            get
            {
                return WDBFieldType.Float;
            }
        }

        public WDBFloatField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "0.0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBCellValidationNames.FLOAT_NAME };
        }
    }
}
