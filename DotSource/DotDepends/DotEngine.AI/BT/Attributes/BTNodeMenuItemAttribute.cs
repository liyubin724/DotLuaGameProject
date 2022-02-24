using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BTNodeMenuItemAttribute : Attribute
    {
        public string MenuPath { get; private set; }
        
        public BTNodeMenuItemAttribute(string path)
        {
            MenuPath = path;
        }
    }
}
