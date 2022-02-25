using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BTNodeMenuItemAttribute : Attribute
    {
        public string Path { get; private set; }
        public string Name { get; private set; }
        
        public BTNodeMenuItemAttribute(string path,string name)
        {
            Path = path;
            Name = name;
        }
    }
}
