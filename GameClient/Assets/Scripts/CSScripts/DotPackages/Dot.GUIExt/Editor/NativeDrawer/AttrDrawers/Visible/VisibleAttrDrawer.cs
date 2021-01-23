using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class VisibleAttrDrawer : INAttrDrawer
    {
        public NDrawerAttribute DrawerAttr { get; set; }

        public T GetAttr<T>() where T : NDrawerAttribute
        {
            return (T)DrawerAttr;
        }

        public abstract bool IsVisible();
    }
}
