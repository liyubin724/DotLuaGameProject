using System;

namespace DotEditor.GUIExt.Field
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited = false)]
    public class TypeDrawableAttribute : Attribute
    {
        public Type ValueType { get; private set; }

        public TypeDrawableAttribute(Type type)
        {
            ValueType = type;
        }
    }
}
