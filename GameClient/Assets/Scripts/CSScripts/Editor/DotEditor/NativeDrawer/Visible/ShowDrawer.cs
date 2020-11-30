using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [AttrDrawBinder(typeof(ShowAttribute))]
    public class ShowDrawer : VisibleDrawer
    {
        public ShowDrawer(VisibleAtrribute attr) : base(attr)
        {
        }

        public override bool IsVisible()
        {
            return true;
        }
    }
}
