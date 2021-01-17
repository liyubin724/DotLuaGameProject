using System;

namespace DotEngine.BehaviourDirector.Nodes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ConditionNodeAttribute : Attribute
    {
        public string Menu { get; private set; }
        public string Label { get; private set; }

        public NodeMode Mode { get; set; } = NodeMode.All;
        public NodePlatform Platform { get; set; } = NodePlatform.All;

        public ConditionNodeAttribute(string menu,string label)
        {
            Menu = menu;
            Label = label;
        }
    }
}
