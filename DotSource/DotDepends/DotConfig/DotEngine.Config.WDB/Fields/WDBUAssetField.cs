namespace DotEngine.Config.WDB
{
    [CustomField("uasset")]
    public class WDBUAssetField : WDBField
    {
        public WDBUAssetField(int column, string type) : base(column, type)
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
