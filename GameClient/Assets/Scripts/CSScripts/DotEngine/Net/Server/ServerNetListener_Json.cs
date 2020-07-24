#if NET_MESSAGE_JSON
using Newtonsoft.Json;
using System.Text;

namespace DotEngine.Net.Server
{
    public partial class ServerNetListener
    {
        public void SendJsonMessage(int netID, int messageID)
        {
            SendData(netID,messageID);
        }

        public void SendJsonMessage<T>(int netID, int messageID, T msg)
        {
            string msgJson = JsonConvert.SerializeObject(msg);
            byte[] msgBytes = null;
            if (!string.IsNullOrEmpty(msgJson))
            {
                msgBytes = Encoding.UTF8.GetBytes(msgJson);
            }
            SendData(netID,messageID, msgBytes);
        }

        public void SendJsonMessage<T>(int netID, int messageID, T msg, bool isCrypto, bool isCompress)
        {
            string msgJson = JsonConvert.SerializeObject(msg);
            byte[] msgBytes = null;
            if (!string.IsNullOrEmpty(msgJson))
            {
                msgBytes = Encoding.UTF8.GetBytes(msgJson);
            }
            SendData(netID,messageID, msgBytes,isCrypto,isCompress);
        }
    }
}
#endif