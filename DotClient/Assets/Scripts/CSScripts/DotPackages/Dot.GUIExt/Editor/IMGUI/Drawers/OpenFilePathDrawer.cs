namespace DotEditor.GUIExt.IMGUI
{
    public class OpenFilePathDrawer : ValueProviderLayoutDrawable<string>
    {
        public bool IsAbsolute { get; set; } = false;
        public bool IsTextEditable { get; set; } = false;
        public string Extension { get; set; } = null;

        //example:[ "Image files", "png,jpg,jpeg", "All files", "*" ]
        public string[] Filters { get; set; } = null;

        protected override void OnLayoutDraw()
        {
            if (Filters != null && Filters.Length > 0)
            {
                Value = EGUILayout.DrawOpenFileWithFilter(Text, Value, Filters, IsAbsolute, IsTextEditable);
            } else
            {
                Value = EGUILayout.DrawOpenFile(Text, Value, Extension, IsAbsolute, IsTextEditable);
            }
        }
    }
}
