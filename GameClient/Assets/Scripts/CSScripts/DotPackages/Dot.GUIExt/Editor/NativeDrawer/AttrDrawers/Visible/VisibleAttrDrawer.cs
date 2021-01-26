using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class VisibleAttrDrawer : IAttrDrawer
    {
        public DrawerAttribute Attr { get; set; }

        public T GetAttr<T>() where T : DrawerAttribute
        {
            return (T)Attr;
        }

        public abstract bool IsVisible();

        public void OnGUILayout()
        {
            throw new System.NotImplementedException();
        }
    }
}
