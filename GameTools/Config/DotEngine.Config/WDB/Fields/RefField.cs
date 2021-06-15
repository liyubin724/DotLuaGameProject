namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Ref)]
    public class RefField : WDBField
    {
        public RefField(int col) : base(col)
        {
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "int" };
        }
    }
}
