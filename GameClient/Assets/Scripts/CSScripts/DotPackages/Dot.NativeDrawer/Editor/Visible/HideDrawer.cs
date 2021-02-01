using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [Binder(typeof(HideAttribute))]
    public class HideDrawer : VisibleDrawer
    {
        public override bool IsVisible()
        {
            return false;
        }
    }
}
