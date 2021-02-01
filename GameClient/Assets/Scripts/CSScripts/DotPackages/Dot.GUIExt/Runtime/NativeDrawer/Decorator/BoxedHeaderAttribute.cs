using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class BoxedHeaderAttribute : DecoratorAttribute
    {
        public string Header { get; private set; }

        public BoxedHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}
