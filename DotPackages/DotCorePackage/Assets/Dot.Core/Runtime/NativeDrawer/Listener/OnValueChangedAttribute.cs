using System;

namespace DotEngine.NativeDrawer.Listener
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =true,Inherited =true)]
    public class OnValueChangedAttribute : ListenerAttribute
    {
        public string MethodName { get; private set; }
        public OnValueChangedAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
