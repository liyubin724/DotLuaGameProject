namespace DotEngine.NativeDrawer.Visible
{
    public abstract class VisibleAtrribute : NativeDrawerAttribute
    {
    }

    public abstract class VisibleCompareAttribute : CompareDrawerAttribute
    {
        protected VisibleCompareAttribute(string memberName, object value, CompareSymbol symbol) : base(memberName, value, symbol)
        {
        }
    }
}
