using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    public abstract class VisibleDrawer : AttrNativeDrawer
    {
        public VisibleDrawer(VisibleAtrribute attr) : base(attr)
        {
            
        }

        public abstract bool IsVisible();
    }

    public abstract class VisibleCompareDrawer : CompareAttrNativeDrawer
    {
        protected VisibleCompareDrawer(object target, VisibleCompareAttribute attr) : base(target, attr)
        {
        }

        public abstract bool IsVisible();
    }
}
