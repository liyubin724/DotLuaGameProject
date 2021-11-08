using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public delegate void ServerStateListener();
    public delegate void ServerMessageListener(Guid guid, int messageId, byte[] messageBody);

    public class ServerNetwork2 : IServerHandler
    {
        private SimpleServer server = null;
        private Dictionary<int, List<ServerMessageListener>> messageListenerDic = new Dictionary<int, List<ServerMessageListener>>();

        private IMessageEncoder messageEncoder;
        private IMessageDecoder messageDecoder;

        public ServerStateListener NetStarted;
        public ServerStateListener NetStopped;
        public ServerStateListener NetError;

        public bool IsStarted
        {
            get
            {
                return server != null && server.IsStarted;
            }
        }

        public ServerNetwork2(IMessageEncoder encoder, IMessageDecoder decoder)
        {
            messageEncoder = encoder;
            messageDecoder = decoder;
        }

        public bool MulticastMessage(int messageId,byte[] messageBody)
        {
            if(!IsStarted)
            {
                return false;
            }
            byte[] dataBytes = messageEncoder.Encode(messageId, messageBody, IsMessageNeedEncrypt(messageId), IsMessageNeedCompress(messageId));
            return server.MulticastMessage(dataBytes);
        }

        public bool MulticastMessageTo(Guid guid,int messageId,byte[] messageBody)
        {
            if (!IsStarted)
            {
                return false;
            }
            byte[] dataBytes = messageEncoder.Encode(messageId, messageBody, IsMessageNeedEncrypt(messageId), IsMessageNeedCompress(messageId));
            return server.MulticastMessageTo(guid, dataBytes);
        }

        private bool IsMessageNeedEncrypt(int messageId)
        {
            return false;
        }

        private bool IsMessageNeedCompress(int messageId)
        {
            return false;
        }

        public void OnMessageReceived(Guid guid, byte[] dataBytes)
        {
            throw new NotImplementedException();
        }


        public void OnStateChanged(ServerState state)
        {
            throw new NotImplementedException();
        }

        #region message listener
        public void RegisterListener(int messageId, ServerMessageListener listener)
        {
            if (!messageListenerDic.TryGetValue(messageId, out var list))
            {
                list = ListPool<ServerMessageListener>.Get();
                messageListenerDic.Add(messageId, list);
            }

            list.Add(listener);
        }

        public void UnRegisterListener(int messageId)
        {
            if (messageListenerDic.TryGetValue(messageId, out var list))
            {
                messageListenerDic.Remove(messageId);
                ListPool<ServerMessageListener>.Release(list);
            }
        }

        public void UnregisterListener(int messageId, ServerMessageListener listener)
        {
            if (messageListenerDic.TryGetValue(messageId, out var list))
            {
                if (list.Remove(listener))
                {
                    if (list.Count == 0)
                    {
                        messageListenerDic.Remove(messageId);
                        ListPool<ServerMessageListener>.Release(list);
                    }
                }
            }
        }

        public void UnRegisterAll()
        {
            foreach (var kvp in messageListenerDic)
            {
                ListPool<ServerMessageListener>.Release(kvp.Value);
            }
            messageListenerDic.Clear();
        }
        #endregion
    }
}
