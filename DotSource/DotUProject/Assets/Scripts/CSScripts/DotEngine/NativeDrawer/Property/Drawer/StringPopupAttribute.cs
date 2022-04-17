using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringPopupAttribute : PropertyContentAttribute
    {
        public string[] Options { get; private set; }

        public string MemberName { get; private set; }

        public bool IsSearchable { get; set; } = false;

        public StringPopupAttribute(string[] options)
        {
            Options = options;
        }

        public StringPopupAttribute(string memberName)
        {
            MemberName = memberName;
        }
    }
}
