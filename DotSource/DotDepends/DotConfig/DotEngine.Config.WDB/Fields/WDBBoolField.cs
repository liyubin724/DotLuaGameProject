﻿namespace DotEngine.Config.WDB
{
    [CustomField(WDBFieldNames.BOOL_NAME)]
    public class WDBBoolField : WDBField
    {
        public WDBBoolField(int column, string type) : base(column, type)
        {
        }

        public override WDBFieldType FieldType
        {
            get
            {
                return WDBFieldType.Bool;
            }
        }

        protected override string GetTypeDefaultContent()
        {
            return "false";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBCellValidationNames.BOOL_NAME };
        }
    }
}
