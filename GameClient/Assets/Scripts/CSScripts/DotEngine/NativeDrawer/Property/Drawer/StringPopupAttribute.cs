using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringPopupAttribute : PropertyContentAttribute
    {
        public string[] Options { get; set; } = new string[0];

        public string MemberName { get; set; } = null;

        public bool IsSearchable { get; set; } = false;

        public StringPopupAttribute()
        {
        }
    }
}
