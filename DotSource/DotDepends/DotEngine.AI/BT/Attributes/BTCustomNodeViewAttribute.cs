using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BTCustomNodeViewAttribute : Attribute
    {
        public Type ViewType { get; private set; }

        public BTCustomNodeViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}
