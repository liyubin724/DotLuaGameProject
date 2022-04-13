using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(ShowAttribute))]
    public class ShowAttrDrawer : VisibleAttrDrawer
    {
        public override bool IsVisible()
        {
            return true;
        }
    }
}
