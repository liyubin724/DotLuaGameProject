using System;

namespace DotEditor.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomAttributeDrawerAttribute : Attribute
    {
        public Type AttrType { get; private set; }
        public CustomAttributeDrawerAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}
