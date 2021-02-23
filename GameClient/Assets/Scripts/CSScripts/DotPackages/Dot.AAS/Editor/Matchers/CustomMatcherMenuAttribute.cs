using System;

namespace DotEditor.AAS.Matchers
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomMatcherMenuAttribute : Attribute
    {
        public string MenuPath { get; private set; }
        public CustomMatcherMenuAttribute(string mPath)
        {
            MenuPath = mPath;
        }
    }
}
