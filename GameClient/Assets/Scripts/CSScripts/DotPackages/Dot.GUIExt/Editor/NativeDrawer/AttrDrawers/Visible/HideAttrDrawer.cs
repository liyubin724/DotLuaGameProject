using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(HideAttribute))]
    public class HideAttrDrawer : VisibleAttrDrawer
    {
        public override bool IsVisible()
        {
            return false;
        }
    }
}
