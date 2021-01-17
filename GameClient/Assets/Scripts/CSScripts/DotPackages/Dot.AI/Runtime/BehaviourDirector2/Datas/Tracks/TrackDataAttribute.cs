using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TrackDataAttribute : Attribute
    {
        public string Label { get; private set; }
        public TrackCategory Category { get; private set; }
        public ActionCategory[] AllowedActionCategories { get; private set; }

        public TrackDataAttribute(string label, TrackCategory category, params ActionCategory[] categories)
        {
            Label = label;
            Category = category;
            AllowedActionCategories = categories;
        }
    }
}
