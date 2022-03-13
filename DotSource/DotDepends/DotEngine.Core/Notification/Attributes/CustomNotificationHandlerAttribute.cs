using System;

namespace DotEngine.Notification
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CustomNotificationHandlerAttribute : Attribute
    {
        public string Name { get; private set; }
        public CustomNotificationHandlerAttribute(string name)
        {
            Name = name;
        }
    }
}
