using System;

namespace DotEngine.Config.WDB
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomValidationAttribute : Attribute
    {
        public string Name { get; private set; }

        public CustomValidationAttribute(string name)
        {
            Name = name;
        }
    }
}
