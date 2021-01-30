namespace DotEngine.NativeDrawer
{
    public abstract class NativeConditionDrawerAttribute : NativeDrawerAttribute
    {

    }

    public enum CompareSymbol
    {
        Eq,
        Neq,
        Lt,
        Gt,
        Lte,
        Gte,
    }

    public abstract class CompareDrawerAttribute : NativeConditionDrawerAttribute
    {
        public string MemberName { get; private set; }
        public CompareSymbol Symbol { get; private set; }
        public object Value { get; private set; }

        protected CompareDrawerAttribute(string memberName, object value, CompareSymbol symbol = CompareSymbol.Eq)
        {
            MemberName = memberName;
            Symbol = symbol;
            Value = value;
        }
    }
}
