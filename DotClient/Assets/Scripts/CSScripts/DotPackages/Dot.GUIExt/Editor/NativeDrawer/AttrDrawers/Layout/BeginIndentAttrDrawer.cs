using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(BeginIndentAttribute))]
    public class BeginIndentAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.BeginIndent();
        }
    }
}
