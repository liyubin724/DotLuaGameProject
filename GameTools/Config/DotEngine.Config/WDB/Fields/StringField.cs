namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.String)]
    public class StringField : WDBField
    {
        public StringField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return string.Empty;
        }
    }
}
