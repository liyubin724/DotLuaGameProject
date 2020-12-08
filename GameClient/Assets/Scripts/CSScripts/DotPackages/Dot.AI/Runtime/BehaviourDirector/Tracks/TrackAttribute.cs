using DotEngine.AI.BD.Actions;
using System;

namespace DotEngine.AI.BD.Tracks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TrackAttribute : Attribute
    {
        public string Label { get; private set; }
        public ActionCategory[] AllowedActionCategories { get; private set; }

        public TrackAttribute(string label,params ActionCategory[] categories)
        {
            Label = label;
            AllowedActionCategories = categories;
        }
    }
}
