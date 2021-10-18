using System;
using SystemObject = System.Object;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =false)]
    public class CompareVisibleAttribute : VisibleAttribute
    {
        public CompareSymbol Symbol { get; private set; }
        public SystemObject Value { get; private set; }
        public string MemberName { get; private set; }

        public CompareVisibleAttribute(string memberName, SystemObject value, CompareSymbol symbol = CompareSymbol.Eq)
        {
            MemberName = memberName;
            Value = value;
            Symbol = symbol;
        }
    }
}
