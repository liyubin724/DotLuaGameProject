using System;

namespace DotEngine.Net
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ClientMessageHandlerAttribute:Attribute
    {
        public int ID { get; set; }
        public ClientMessageHandlerAttribute(int id)
        {
            ID = id;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ServerMessageHandlerAttribute : Attribute
    {
        public int ID { get; set; }
        public ServerMessageHandlerAttribute(int id)
        {
            ID = id;
        }
    }
}
