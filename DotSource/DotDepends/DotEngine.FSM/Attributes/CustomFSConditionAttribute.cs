using System;

namespace DotEngine.FSM
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomFSConditionAttribute : Attribute
    {
        public string MenuName { get; private set; }
        public string MenuPath { get; private set; }

        public CustomFSConditionAttribute(string name, string path)
        {
            MenuName = name;
            MenuPath = path;
        }
    }
}
