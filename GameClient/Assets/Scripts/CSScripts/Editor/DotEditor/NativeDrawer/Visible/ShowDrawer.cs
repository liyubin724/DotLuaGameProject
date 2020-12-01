using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [Binder(typeof(ShowAttribute))]
    public class ShowDrawer : VisibleDrawer
    {
        public override bool IsVisible()
        {
            return true;
        }
    }
}
