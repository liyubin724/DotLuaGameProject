using DotEngine.NativeDrawer.Layout;

namespace DotEditor.NativeDrawer.Layout
{
    public abstract class LayoutDrawer : AttrDrawer
    {
        protected LayoutDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public abstract void OnGUILayout();
    }
}
