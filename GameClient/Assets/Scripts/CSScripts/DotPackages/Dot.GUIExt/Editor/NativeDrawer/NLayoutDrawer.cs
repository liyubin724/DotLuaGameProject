using DotEditor.GUIExt.Layout;

namespace DotEditor.GUIExt.NativeDrawer
{
    public interface INLayoutDrawer : ILayoutDrawable
    {
    }

    public abstract class NLayoutDrawer : INLayoutDrawer
    {
        public abstract void OnGUILayout();
    }
}