using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntPopupAttribute : PropertyDrawerAttribute
    {
        public int[] Values { get; set; } = new int[0];
        public string[] Contents { get; set; } = new string[0];

        public string ValueMemberName { get; set; } = null;
        public string ContentMemberName { get; set; } = null;

        public bool IsSearchable { get; set; } = false;

        public IntPopupAttribute()
        {
        }
    }
}
