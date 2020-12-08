using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ActionDataAttribute : Attribute
    {
        public string MenuPrefix { get; private set; }
        public string Label { get; private set; }
        public ActionCategory[] Categories { get; private set; }

        public DataTarget Target { get; set; } = DataTarget.All;
        public DataMode Model { get; set; } = DataMode.All;

        public ActionDataAttribute(string menu,string label,params ActionCategory[] categories)
        {
            MenuPrefix = menu;
            Label = label;
            Categories = categories;
        }
    }
}
