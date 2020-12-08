using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TrackDataAttribute : Attribute
    {
        public string Label { get; private set; }
        public ActionCategory[] AllowedActionCategories { get; private set; }

        public TrackDataAttribute(string label, params ActionCategory[] categories)
        {
            Label = label;
            AllowedActionCategories = categories;
        }
    }
}
