using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ActionDataAttribute : Attribute
    {
        public string MenuPrefix { get; private set; }
        public string Label { get; private set; }
        public ActionCategory[] Categories { get; private set; }

        public BDDataTarget Target { get; set; } = BDDataTarget.All;
        public BDDataMode Model { get; set; } = BDDataMode.All;

        public ActionDataAttribute(string menu,string label,params ActionCategory[] categories)
        {
            MenuPrefix = menu;
            Label = label;
            Categories = categories;
        }
    }
}
