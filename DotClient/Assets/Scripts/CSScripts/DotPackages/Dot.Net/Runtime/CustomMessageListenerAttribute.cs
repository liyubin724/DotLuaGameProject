using System;

namespace DotEngine.Net
{
    public enum MessageListenerType
    {
        Client = 0,
        Server,
    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false,Inherited = true)]
    public class CustomMessageListenerAttribute : Attribute
    {
        public int MessageId { get; private set; }
        public MessageListenerType ListenerType { get; private set; }

        public CustomMessageListenerAttribute(int id,MessageListenerType lType)
        {
            MessageId = id;
            ListenerType = lType;
        }
    }
}
