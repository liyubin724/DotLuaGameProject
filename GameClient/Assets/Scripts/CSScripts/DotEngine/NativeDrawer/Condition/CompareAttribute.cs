namespace DotEngine.NativeDrawer.Condition
{
    public enum CompareSymbol
    {
        Eq,
        Neq,
        Lt,
        Gt,
        Lte,
        Gte,
    }

    public abstract class CompareAttribute : ConditionAttribute
    {
        public string MemberName { get; private set; }
        public CompareSymbol Symbol { get; private set; }
        public object Value { get; private set; }

        protected CompareAttribute(string memberName, object value, CompareSymbol symbol = CompareSymbol.Eq)
        {
            MemberName = memberName;
            Symbol = symbol;
            Value = value;
        }
    }
}
