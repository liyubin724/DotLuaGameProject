using System;

namespace DotEditor.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AttrDrawBinderAttribute : Attribute
    {
        public Type AttrType { get; private set; }
        public AttrDrawBinderAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}
