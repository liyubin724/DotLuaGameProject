using System;

namespace DotEngine.AI.BD.Actions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ActionAttribute : Attribute
    {
        public string MenuPrefix { get; private set; }
        public string Label { get; private set; }
        public ActionCategory[] Categories { get; private set; }

        public ActionAttribute(string menu,string label,params ActionCategory[] categories)
        {
            MenuPrefix = menu;
            Label = label;
            Categories = categories;
        }
    }
}
