namespace DotEditor.GUIExt.NativeDrawer
{
    public class BeginIndentAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.BeginIndent();
        }
    }
}
