namespace DotEngine.GUIExt.NativeDrawer
{
    public class CompareVerificationAttribute : VerificationAttribute
    {
        public CompareSymbol Symbol { get; private set; }

        public object Value { get; private set; }

        public string MemberName { get; private set; }

        public CompareVerificationAttribute(string invalidMsg, object value, CompareSymbol symbol = CompareSymbol.Eq) : base(invalidMsg)
        {
            Symbol = symbol;
            Value = value;
        }

        public CompareVerificationAttribute(string invalidMsg, string memberName, CompareSymbol symbol = CompareSymbol.Eq) : base(invalidMsg)
        {
            Symbol = symbol;
            MemberName = memberName;
        }
    }
}
