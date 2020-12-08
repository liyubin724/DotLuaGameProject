using DotEngine.AI.BD.Tracks;
using System;

namespace DotEngine.AI.BD
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false,Inherited =false)]
    public class TrackGroupAttribute : Attribute
    {
        public string Label { get; private set; }
        public TrackCategory[] AllowedTrackCategories { get; private set; }

        public TrackGroupAttribute(string label, params TrackCategory[] categories)
        {
            Label = label;
            AllowedTrackCategories = categories;
        }
    }
}
