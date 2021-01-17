using System;

namespace DotEngine.BehaviourDirector.Nodes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ActionNodeAttribute : Attribute
    {
        public string MenuPrefix { get; private set; }
        public string Label { get; private set; }

        public NodeMode Model { get; set; } = NodeMode.All;
        public NodePlatform Platform { get; set; } = NodePlatform.All;
        public ActionNodeCategory[] Categories { get; set; } = null;

        public bool IsFixedDuration { get; set; } = false;

        public ActionNodeAttribute(string menu, string label,params ActionNodeCategory[] categories)
        {
            MenuPrefix = menu;
            Label = label;
            Categories = categories;
        }
    }
}
