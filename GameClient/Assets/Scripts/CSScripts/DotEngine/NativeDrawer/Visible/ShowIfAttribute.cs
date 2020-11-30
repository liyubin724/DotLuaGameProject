using DotEngine.NativeDrawer.Condition;
using System;

namespace DotEngine.NativeDrawer.Visible
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : VisibleCompareAttribute
    {
        public ShowIfAttribute(string memberName, object value, CompareSymbol symbol = CompareSymbol.Eq) : base(memberName, value, symbol)
        {
        }
    }
}
