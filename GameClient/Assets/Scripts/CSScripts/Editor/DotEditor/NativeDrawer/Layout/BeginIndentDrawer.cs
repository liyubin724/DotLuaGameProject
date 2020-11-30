using DotEngine.NativeDrawer.Layout;
using DotEditor.GUIExtension;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrBinder(typeof(BeginIndentAttribute))]
    public class BeginIndentDrawer : LayoutDrawer
    {
        public BeginIndentDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public override void OnGUILayout()
        {
            EGUI.BeginIndent();
        }
    }
}
