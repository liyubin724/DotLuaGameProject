using System;

namespace DotEngine.BehaviourDirector.Nodes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class TrackNodeAttribute : Attribute
    {
        public string Label { get; private set; }
        public ActionNodeCategory[] ActionCategories { get; set; } = null;

        public TrackNodeAttribute(string label,params ActionNodeCategory[] actionNodeCategories)
        {
            Label = label;
            ActionCategories = actionNodeCategories;
        }
    }
}
