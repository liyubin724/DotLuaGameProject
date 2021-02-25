using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(OpenFolderPathAttribute))]
    public class OpenFolderPathAttrDrawer : ContentAttrDrawer
    {
        protected override void DrawContent()
        {
            OpenFolderPathAttribute attr = GetAttr<OpenFolderPathAttribute>();
            ItemDrawer.Value = EGUILayout.DrawOpenFolder(ItemDrawer.Label, (string)ItemDrawer.Value, attr.IsAbsolute, !attr.CanEditText);
        }

        protected override bool IsValidValueType()
        {
            return ItemDrawer.ValueType == typeof(string);
        }
    }
}
