﻿namespace DotEngine.Config.WDB
{
    [CustomField("ref")]
    public class WDBRefField : WDBField
    {
        public WDBRefField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "-1";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBValidationNames.INT_NAME };
        }
    }
}