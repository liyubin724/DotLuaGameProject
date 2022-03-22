using System;

namespace DotEngine.UIElements
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class QAttribute : Attribute
    {
        public string Name { get; set; }
        public string[] Classes { get; set; }

        public QAttribute()
        {
        }

        public QAttribute(string name, params string[] classes)
        {
            Name = name;
            Classes = classes;
        }
    }
}
