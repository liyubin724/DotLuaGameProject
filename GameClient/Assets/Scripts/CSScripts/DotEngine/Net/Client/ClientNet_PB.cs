#if NET_MESSAGE_PB
using Google.Protobuf;

namespace Dot.Net.Client
{
    public partial class ClientNet
    {
        public void SendPBMessage(int messageID)
        {
            if (IsConnected())
            {
                SendData(messageID);
            }
        }

        public void SendPBMessage<T>(int messageID, T msg) where T:IMessage
        {
            if (IsConnected())
            {
                byte[] msgBytes = null;
                if (msg!=null)
                {
                    msgBytes = msg.ToByteArray();
                }
                SendData(messageID, msgBytes);
            }
        }
    }
}
#endif
