using System;

namespace DotEngine.NetCore.TCPNetwork
{
    public class MessageHandlerAttribute : Attribute
    {
        public int MsgID { get; private set; }
        public MessageHandlerAttribute(int msgID)
        {
            MsgID = msgID;
        }
    }
}
