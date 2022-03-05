using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BTNodeIdentificationAttribute : Attribute
    {
        public uint DataId { get; private set; }
        public string DisplayName { get; private set; }
        public string Tooltips { get; set; } = string.Empty;

        public BTNodeIdentificationAttribute(uint dataId, string dispalyName)
        {
            DataId = dataId;
            DisplayName = dispalyName;
        }
    }
}
