using DotEngine.NativeDrawer;

namespace DotEditor.NativeDrawer
{
    public abstract class Drawer
    {
        public DrawerAttribute Attr { get; set; }
        public DrawerProperty Property { get; set; }

        public T GetAttr<T>() where T:DrawerAttribute
        {
            return (T)Attr;
        }
    }
}
