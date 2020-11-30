using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Layout;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrBinder(typeof(BeginIndentAttribute))]
    public class BeginIndentDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.BeginIndent();
        }
    }
}
