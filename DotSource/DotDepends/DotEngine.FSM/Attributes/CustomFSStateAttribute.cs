using System;

namespace DotEngine.FSM
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomFSStateAttribute : Attribute
    {
        public string MenuName { get; private set; }
        public string MenuPath { get; private set; }

        public CustomFSStateAttribute(string name,string path)
        {
            MenuName = name;
            MenuPath = path;
        }
    }
}
