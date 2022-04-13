namespace DotEditor.GUIExt.IMGUI
{
    public class OpenFolderPathDrawer : ValueProviderLayoutDrawable<string>
    {
        public bool IsAbsolute { get; set; } = false;

        protected override void OnLayoutDraw()
        {
            Value = EGUILayout.DrawOpenFolder(Text, Value, IsAbsolute);
        }
    }
}
