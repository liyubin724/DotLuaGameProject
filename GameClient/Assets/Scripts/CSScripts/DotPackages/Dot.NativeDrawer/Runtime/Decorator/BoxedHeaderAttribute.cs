using System;

namespace DotEngine.NativeDrawer.Decorator
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
