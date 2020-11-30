using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [AttrDrawBinder(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : VisibleCompareDrawer
    {
        public ShowIfDrawer(object target, VisibleCompareAttribute attr) : base(target, attr)
        {
        }

        public override bool IsVisible()
        {
            return IsEqual();
        }
    }
}
