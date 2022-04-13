using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class TypeDrawer : LayoutDrawer
    {
        public string Label { get; set; }
        public ItemDrawer ItemDrawer { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomTypeDrawerAttribute : Attribute
    {
        public Type TargetType { get; private set; }

        public CustomTypeDrawerAttribute(Type type)
        {
            TargetType = type;
        }
    }
}