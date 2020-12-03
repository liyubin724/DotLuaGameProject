#if NET_MESSAGE_PB
using Google.Protobuf;
namespace DotEngine.Net.Server
{
    public partial class ServerNetListener
    {
        public void SendPBMessage(int netID,int messageID)
        {
            SendData(netID,messageID);
        }

        public void SendPBMessage<T>(int netID, int messageID, T msg) where T : IMessage
        {
            byte[] msgBytes = null;
            if (msg != null)
            {
                msgBytes = msg.ToByteArray();
            }
            SendData(netID, messageID, msgBytes);
        }
        public void SendPBMessage<T>(int netID, int messageID, T msg, bool isCrypto, bool isCompress) where T : IMessage
        {
            byte[] msgBytes = null;
            if (msg != null)
            {
                msgBytes = msg.ToByteArray();
            }
            SendData(netID,messageID, msgBytes,isCrypto,isCompress);
        }
    }
}
#endif