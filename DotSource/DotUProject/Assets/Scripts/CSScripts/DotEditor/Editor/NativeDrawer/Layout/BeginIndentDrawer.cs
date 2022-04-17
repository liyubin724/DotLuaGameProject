using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Layout;

namespace DotEditor.NativeDrawer.Layout
{
    [Binder(typeof(BeginIndentAttribute))]
    public class BeginIndentDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.BeginIndent();
        }
    }
}
