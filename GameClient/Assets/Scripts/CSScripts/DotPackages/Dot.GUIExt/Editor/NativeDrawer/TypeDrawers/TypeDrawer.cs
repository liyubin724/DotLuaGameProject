using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class TypeDrawer : NLayoutDrawer
    {
        public string Label { get; set; }
        public NItemDrawer ItemDrawer { get; set; }
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