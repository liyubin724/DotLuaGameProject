using System;

namespace DotEditor.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomTypeDrawerAttribute : Attribute
    {
        public Type Target { get; private set; }

        public CustomTypeDrawerAttribute(Type target)
        {
            Target = target;
        }
    }
}
