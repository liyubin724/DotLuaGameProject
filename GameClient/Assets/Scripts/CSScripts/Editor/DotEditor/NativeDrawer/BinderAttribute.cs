using System;

namespace DotEditor.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BinderAttribute : Attribute
    {
        public Type AttrType { get; private set; }
        public BinderAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}
