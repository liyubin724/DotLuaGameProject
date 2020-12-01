using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    public abstract class VisibleDrawer : Drawer
    {
        public abstract bool IsVisible();
    }

    //public abstract class VisibleCompareDrawer : CompareAttrDrawer
    //{
    //    protected VisibleCompareDrawer(object target, VisibleCompareAttribute attr) : base(target, attr)
    //    {
    //    }

    //    public abstract bool IsVisible();
    //}
}
