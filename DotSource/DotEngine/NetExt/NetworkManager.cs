using DotEngine.Log;
using DotEngine.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.NetExt
{
    public delegate void NetworkMessageHandler(int messageId, byte[] messageBytes);

    public class NetworkManager : Singleton<NetworkManager>
    {
        private const string LOG_TAG = "NetManager";

        public ClientNetwork clientNetwork;
        private Dictionary<int, List<NetworkMessageHandler>> handlerDic;

        protected override void OnInit()
        {
            handlerDic = new Dictionary<int, List<NetworkMessageHandler>>();
        }

        protected override void OnDestroy()
        {
            if(clientNetwork!=null && clientNetwork.IsConnected)
            {
                clientNetwork.Dispose();
                clientNetwork = null;
            }

            base.OnDestroy();
        }

        public void Connect(string ip,int port)
        {
            if(clientNetwork!=null)
            {
                clientNetwork.Dispose();
                clientNetwork = null;
            }

            clientNetwork = new ClientNetwork(ip, port);
            clientNetwork.OnDataReceived = OnDataReceived;
            
        }

        public void RegisterMessageHandler(int messageId,NetworkMessageHandler handler)
        {
            if(!handlerDic.TryGetValue(messageId,out var list))
            {
                list = new List<NetworkMessageHandler>();
                handlerDic.Add(messageId, list);
            }

            if(!list.Contains(handler))
            {
                list.Add(handler);
            }
        }

        public void UnregisterMessageHandler(int messageId,NetworkMessageHandler handler)
        {
            if(handlerDic.TryGetValue(messageId,out var list) && list.Contains(handler))
            {
                list.Remove(handler);
            }
        }

        public void SendMessage(int messId, byte[] messBytes)
        {
            if(clientNetwork == null || !clientNetwork.IsConnected)
            {
                return;
            }

            int dataLen = sizeof(int);
            if (messBytes != null && messBytes.Length > 0)
            {
                dataLen += messBytes.Length;
            }
            byte[] lenBytes = BitConverter.GetBytes(dataLen);
            dataLen += sizeof(int);
            byte[] messIdBytes = BitConverter.GetBytes(messId);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(messIdBytes, dataBytes, sizeof(int));
            if (messBytes != null && messBytes.Length > 0)
            {
                Array.Copy(messBytes, 0, dataBytes, sizeof(int) + sizeof(int), messBytes.Length);
            }
            clientNetwork.Send(dataBytes, 0, dataLen);
        }

        public void SendMessageAsync(int messId, byte[] messBytes)
        {
            if (clientNetwork == null || !clientNetwork.IsConnected)
            {
                return;
            }

            int dataLen = sizeof(int);
            if (messBytes != null && messBytes.Length > 0)
            {
                dataLen += messBytes.Length;
            }
            byte[] lenBytes = BitConverter.GetBytes(dataLen);
            dataLen += sizeof(int);
            byte[] messIdBytes = BitConverter.GetBytes(messId);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(messIdBytes, dataBytes, sizeof(int));
            if (messBytes != null && messBytes.Length > 0)
            {
                Array.Copy(messBytes, 0, dataBytes, sizeof(int) + sizeof(int), messBytes.Length);
            }
            clientNetwork.SendAsync(dataBytes, 0, dataLen);
        }

        private void OnDataReceived(byte[] dataBytes)
        {
            int messageId = BitConverter.ToInt32(dataBytes, 0);
            if(handlerDic.TryGetValue(messageId,out var list))
            {
                Byte[] messageBytes = null;
                if(dataBytes.Length>0)
                {
                    messageBytes = new byte[dataBytes.Length - sizeof(int)];
                    Array.Copy(dataBytes, sizeof(int), messageBytes, 0, messageBytes.Length);
                }
                var handlers = list.ToArray();
                foreach(var handler in handlers)
                {
                    handler?.Invoke(messageId, messageBytes);
                }
            }
            else
            {
                LogUtil.Warning(LOG_TAG, $"the handler is not found for the messageId{messageId}.");
            }
        }
    }
}
