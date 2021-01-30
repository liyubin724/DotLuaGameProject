#if NET_MESSAGE_JSON
using Newtonsoft.Json;
using System.Text;

namespace Dot.Net.Client
{
    public partial class ClientNet
    {
        public void SendJsonMessage(int messageID)
        {
            if(IsConnected())
            {
                SendData(messageID);
            }
        }

        public void SendJsonMessage<T>(int messageID,T msg)
        {
            if(IsConnected())
            {
                string msgJson = JsonConvert.SerializeObject(msg);
                byte[] msgBytes = null;
                if(!string.IsNullOrEmpty(msgJson))
                {
                    msgBytes = Encoding.UTF8.GetBytes(msgJson);
                }
                SendData(messageID, msgBytes);
            }
        }
    }
}
#endif
