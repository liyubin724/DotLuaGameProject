using System;

namespace DotEngine.Config.WDB
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomFieldAttribute : Attribute
    {
        public string FieldType { get; private set; }

        public CustomFieldAttribute(string fieldType)
        {
            FieldType = fieldType;
        }
    }
}