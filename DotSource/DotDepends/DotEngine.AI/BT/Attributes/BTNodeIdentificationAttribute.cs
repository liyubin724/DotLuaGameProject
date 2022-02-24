using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BTNodeIdentificationAttribute : Attribute
    {
        public string Name { get; private set; }
        public uint DataId { get; private set; }

        public BTNodeIdentificationAttribute(string name,uint dataId)
        {
            Name = name;
            DataId = dataId;
        }
    }
}
