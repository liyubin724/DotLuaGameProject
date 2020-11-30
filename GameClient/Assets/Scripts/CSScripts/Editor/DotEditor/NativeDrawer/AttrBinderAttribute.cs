using System;

namespace DotEditor.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttrBinderAttribute : Attribute
    {
        public Type AttrType { get; private set; }
        public AttrBinderAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}
