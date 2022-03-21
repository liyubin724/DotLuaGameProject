using System;

namespace DotEngine.FSM
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomStateAttribute : Attribute
    {
        public int UniqueId { get; private set; }
        public string MenuName { get; private set; }
        public string MenuPath { get; private set; }

        public CustomStateAttribute(int id, string name,string path)
        {
            UniqueId = id;
            MenuName = name;
            MenuPath = path;
        }
    }
}
