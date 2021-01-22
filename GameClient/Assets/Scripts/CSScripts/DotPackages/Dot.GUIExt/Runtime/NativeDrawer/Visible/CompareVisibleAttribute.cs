using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =false)]
    public class CompareVisibleAttribute : VisibleAtrribute
    {
        public CompareSymbol Symbol { get; private set; }
        public object Value { get; private set; }
        public string MemberName { get; private set; }

        public CompareVisibleAttribute(object value, CompareSymbol symbol = CompareSymbol.Eq)
        {
            Symbol = symbol;
            Value = value;
        }

        public CompareVisibleAttribute(string memberName, CompareSymbol symbol = CompareSymbol.Eq)
        {
            Symbol = symbol;
            MemberName = memberName;
        }
    }
}
