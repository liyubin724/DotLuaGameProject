using DotEngine.NativeDrawer.Condition;

namespace DotEngine.NativeDrawer.Visible
{
    public abstract class VisibleAtrribute : NativeDrawerAttribute
    {
    }

    public abstract class VisibleCompareAttribute : CompareAttribute
    {
        protected VisibleCompareAttribute(string memberName, object value, CompareSymbol symbol) : base(memberName, value, symbol)
        {
        }
    }
}
