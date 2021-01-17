using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false,Inherited =false)]
    public class GroupDataAttribute : Attribute
    {
        public string Label { get; private set; }
        public TrackCategory[] AllowedTrackCategories { get; private set; }

        public GroupDataAttribute(string label, params TrackCategory[] categories)
        {
            Label = label;
            AllowedTrackCategories = categories;
        }
    }
}
