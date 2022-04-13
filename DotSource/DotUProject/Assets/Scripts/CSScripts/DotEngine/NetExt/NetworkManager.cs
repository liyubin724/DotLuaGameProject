using DotEngine.Log;
using DotEngine.Net;
using System;
using System.Collections.Generic;

namespace DotEngine.NetExt
{
    public delegate void NetworkMessageHandler(int messageId, byte[] messageBytes);

    public class NetworkManager : Singleton<NetworkManager>
    {
        private const string LOG_TAG = "NetManager";

        public ClientNetwork clientNetwork;
        private Dictionary<int, List<NetworkMessageHandler>> handlerDic;

        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action OnError;

        protected override void OnInit()
        {
            handlerDic = new Dictionary<int, List<NetworkMessageHandler>>();
        }

        protected override void OnDestroy()
        {
            if (clientNetwork != null && clientNetwork.IsConnected)
            {
                clientNetwork.Dispose();
                clientNetwork = null;
            }

            base.OnDestroy();
        }

        public void Connect(string ip, int port)
        {
            if (clientNetwork != null)
            {
                clientNetwork.Dispose();
                clientNetwork = null;
            }

            clientNetwork = new ClientNetwork(ip, port);
            clientNetwork.OnNetConnected = () =>
            {
                OnConnected?.Invoke();
            };
            clientNetwork.OnNetDisconnected = () =>
            {
                OnDisconnected?.Invoke();
            };
            clientNetwork.OnNetError = () =>
            {
                OnError?.Invoke();
            };
            clientNetwork.OnDataReceived = OnDataReceived;
            clientNetwork.ConnectAsync();
        }

        public void Reconnect()
        {
            if (clientNetwork != null && clientNetwork.IsConnected)
            {
                return;
            }

            clientNetwork.ReconnectAsync();
        }

        public void Disconnect()
        {
            if (clientNetwork == null || !clientNetwork.IsConnected)
            {
                return;
            }
            clientNetwork.DisconnectAsync();
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if(clientNetwork!=null && clientNetwork.IsConnected)
            {
                clientNetwork.DoUpdate(deltaTime);
            }
        }

        public void RegisterMessageHandler(int messageId, NetworkMessageHandler handler)
        {
            if (!handlerDic.TryGetValue(messageId, out var list))
            {
                list = new List<NetworkMessageHandler>();
                handlerDic.Add(messageId, list);
            }

            if (!list.Contains(handler))
            {
                list.Add(handler);
            }
        }

        public void UnregisterMessageHandler(int messageId, NetworkMessageHandler handler)
        {
            if (handlerDic.TryGetValue(messageId, out var list) && list.Contains(handler))
            {
                list.Remove(handler);
            }
        }

        public void SendMessage(int messageId, byte[] messageBytes)
        {
            if (clientNetwork == null || !clientNetwork.IsConnected)
            {
                return;
            }

            int dataLen = sizeof(int);
            if (messageBytes != null && messageBytes.Length > 0)
            {
                dataLen += messageBytes.Length;
            }
            byte[] lenBytes = BitConverter.GetBytes(dataLen);
            dataLen += sizeof(int);
            byte[] messIdBytes = BitConverter.GetBytes(messageId);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(messIdBytes, dataBytes, sizeof(int));
            if (messageBytes != null && messageBytes.Length > 0)
            {
                Array.Copy(messageBytes, 0, dataBytes, sizeof(int) + sizeof(int), messageBytes.Length);
            }
            clientNetwork.Send(dataBytes, 0, dataLen);
        }

        public void SendMessageAsync(int messageId, byte[] messageBytes)
        {
            if (clientNetwork == null || !clientNetwork.IsConnected)
            {
                return;
            }

            int dataLen = sizeof(int);
            if (messageBytes != null && messageBytes.Length > 0)
            {
                dataLen += messageBytes.Length;
            }
            byte[] lenBytes = BitConverter.GetBytes(dataLen);
            dataLen += sizeof(int);
            byte[] messIdBytes = BitConverter.GetBytes(messageId);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(messIdBytes, dataBytes, sizeof(int));
            if (messageBytes != null && messageBytes.Length > 0)
            {
                Array.Copy(messageBytes, 0, dataBytes, sizeof(int) + sizeof(int), messageBytes.Length);
            }
            clientNetwork.SendAsync(dataBytes, 0, dataLen);
        }

        private void OnDataReceived(byte[] dataBytes)
        {
            int messageId = BitConverter.ToInt32(dataBytes, 0);
            if (handlerDic.TryGetValue(messageId, out var list))
            {
                Byte[] messageBytes = null;
                if (dataBytes.Length > 0)
                {
                    messageBytes = new byte[dataBytes.Length - sizeof(int)];
                    Array.Copy(dataBytes, sizeof(int), messageBytes, 0, messageBytes.Length);
                }
                var handlers = list.ToArray();
                foreach (var handler in handlers)
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
