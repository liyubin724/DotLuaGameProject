namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Text)]
    public class TextField : WDBField
    {
        public TextField(int col) : base(col)
        {
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "int" };
        }
    }
}
