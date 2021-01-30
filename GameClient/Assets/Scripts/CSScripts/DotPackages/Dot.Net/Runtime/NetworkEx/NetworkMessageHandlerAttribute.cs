using System;

namespace DotEngine.NetworkEx
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public class ClientNetworkMessageHandlerAttribute : Attribute
    {
        public int ID { get; set; }
        public ClientNetworkMessageHandlerAttribute(int id)
        {
            ID = id;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ServerNetworkMessageHandlerAttribute : Attribute
    {
        public int ID { get; set; }
        public ServerNetworkMessageHandlerAttribute(int id)
        {
            ID = id;
        }
    }
}
