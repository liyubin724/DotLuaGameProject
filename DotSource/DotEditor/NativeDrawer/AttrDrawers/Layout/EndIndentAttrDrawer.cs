using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(EndIndentAttribute))]
    public class EndIndentAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.EndIndent();
        }
    }
}
