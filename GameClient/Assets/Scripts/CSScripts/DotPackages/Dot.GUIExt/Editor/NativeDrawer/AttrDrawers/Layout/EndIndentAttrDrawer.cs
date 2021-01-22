namespace DotEditor.GUIExt.NativeDrawer
{
    public class EndIndentAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.EndIndent();
        }
    }
}
