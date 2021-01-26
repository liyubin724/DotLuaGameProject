using UnityEditor;

namespace DotEditor.GUIExt.IMGUI
{
    public class ToolbarButtonDrawer : ButtonDrawer
    {
        public ToolbarButtonDrawer()
        {
            Width = 60.0f;
            Style = EditorStyles.toolbarButton;
        }
    }
}