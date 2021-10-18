using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringPopupAttribute : ContentAttribute
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
