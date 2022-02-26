using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited = false)]
    public class BTNodeLinkedDataAttribute : Attribute
    {
        public Type DataType { get; private set; }
        public BTNodeLinkedDataAttribute(Type dataType)
        {
            DataType = dataType;
        }
    }
}
