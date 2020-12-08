using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false,Inherited =false)]
    public class TrackGroupDataAttribute : Attribute
    {
        public string Label { get; private set; }
        public TrackCategory[] AllowedTrackCategories { get; private set; }

        public TrackGroupDataAttribute(string label, params TrackCategory[] categories)
        {
            Label = label;
            AllowedTrackCategories = categories;
        }
    }
}
