using DotEditor.GUIExt.IMGUI;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class LayoutDrawer : ILayoutDrawable
    {
        public abstract void OnGUILayout();
    }
}