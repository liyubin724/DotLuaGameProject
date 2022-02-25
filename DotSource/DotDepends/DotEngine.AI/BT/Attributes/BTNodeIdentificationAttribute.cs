using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BTNodeIdentificationAttribute : Attribute
    {
        public uint DataId { get; private set; }
        public string Name { get; private set; }
        public string Tooltips { get; set; } = string.Empty;

        public BTNodeIdentificationAttribute(uint dataId, string name)
        {
            DataId = dataId;
            Name = name;
        }
    }
}
