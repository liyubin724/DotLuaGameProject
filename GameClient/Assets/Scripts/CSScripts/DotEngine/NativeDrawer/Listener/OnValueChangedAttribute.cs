using System;

namespace DotEngine.NativeDrawer.Listener
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class OnValueChangedAttribute : ListenerAttribute
    {
        public string CallbackMethodName { get; private set; }

        public OnValueChangedAttribute(string methodName)
        {
            CallbackMethodName = methodName;
        }
    }
}
