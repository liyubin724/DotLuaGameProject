using System;

namespace DotEditor.AAS.Reprocessor
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomReprocessMenuAttribute : Attribute
    {
        public string MenuPath { get; private set; }
        public CustomReprocessMenuAttribute(string mPath)
        {
            MenuPath = mPath;
        }
    }
}
