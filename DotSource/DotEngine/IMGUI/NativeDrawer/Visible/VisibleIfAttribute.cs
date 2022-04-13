using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class VisibleIfAttribute : VisibleAttribute
    {
        public string MemberName { get; private set; }

        public VisibleIfAttribute(string memberName)
        {
            MemberName = memberName;
        }
    }
}
