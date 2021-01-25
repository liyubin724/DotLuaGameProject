using DotEngine.GUIExt.NativeDrawer;
using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public interface INAttrDrawer
    {
        NDrawerAttribute DrawerAttr { get; set; }
        T GetAttr<T>() where T : NDrawerAttribute;
    }

    public abstract class AttrDrawer : LayoutDrawer,INAttrDrawer
    {
        public NDrawerAttribute DrawerAttr { get; set; }

        public T GetAttr<T>() where T : NDrawerAttribute
        {
            return (T)DrawerAttr;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomAttrDrawerAttribute : Attribute
    {
        public Type AttrType { get; private set; }

        public CustomAttrDrawerAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}