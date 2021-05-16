namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Id)]
    public class IdField : WDBField
    {
        public IdField(int col) : base(col)
        {
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "int", "NotEmpty"};
        }
    }
}
