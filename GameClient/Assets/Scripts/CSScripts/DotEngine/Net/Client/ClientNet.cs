using DotEngine.Log;
using DotEngine.Net.Message;
using System.Collections.Generic;

namespace DotEngine.Net.Client
{
    public delegate void ClientNetStateChanged(ClientNet clientNet);

    public interface IClientNetMessageHandler
    {
        ClientNet Net { get; set; }
        bool OnMessageHanlder(int messageID, object message);
    }

    public partial class ClientNet //: IDispose
    {
        private int uniqueID = -1;
        public int UniqueID { get => uniqueID; }

        private MessageWriter messageWriter = null;
        private MessageReader messageReader = null;
        private IMessageParser messageParser = null;

        private ClientNetSession netSession = null;
        private ClientNetSessionState sessionState = ClientNetSessionState.Unavailable;

        private Dictionary<string, IClientNetMessageHandler> messageHandlerDic = new Dictionary<string, IClientNetMessageHandler>();
        
        public event ClientNetStateChanged NetConnecting;
        public event ClientNetStateChanged NetConnectedSuccess;
        public event ClientNetStateChanged NetConnectedFailed;
        public event ClientNetStateChanged NetDisconnected;

        public ClientNet(int id,IMessageParser messageParser)
        {
            uniqueID = id;
            this.messageParser = messageParser;
            messageWriter = new MessageWriter();
            messageReader = new MessageReader();

            netSession = new ClientNetSession(messageReader);
            messageReader.MessageError = OnMessageError;
            messageReader.MessageReceived = OnMessageReceived;
        }

        public bool IsConnected()
        {
            return netSession.IsConnected();
        }

        public void Connect(string address)
        {
            netSession.Connect(address);
        }

        public void Connect(string ip, int port)
        {
            netSession.Connect(ip, port);
        }

        public void Reconnect()
        {
            netSession.Reconnect();
        }

        public void Disconnect()
        {
            netSession.Disconnect();
        }

        public void DoUpdate(float deltaTime)
        {
            ClientNetSessionState currentSessionState = netSession.State;
            if (currentSessionState != sessionState)
            {
                sessionState = currentSessionState;

                if (currentSessionState == ClientNetSessionState.Connecting)
                {
                    NetConnecting?.Invoke(this);
                }
                else if (currentSessionState == ClientNetSessionState.Normal)
                {
                    NetConnectedSuccess?.Invoke(this);
                }
                else if (currentSessionState == ClientNetSessionState.ConnectedFailed)
                {
                    NetConnectedFailed?.Invoke(this);
                }
                else if (currentSessionState == ClientNetSessionState.Disconnected)
                {
                    NetDisconnected?.Invoke(this);
                }
            }
        }

        public void DoLateUpdate()
        {
            if (IsConnected())
            {
                netSession.DoLateUpdate();
            }
        }

        public void Dispose()
        {
            messageReader.MessageError = null;
            messageReader.MessageReceived = null;

            messageHandlerDic.Clear();

            netSession.Dispose();
            netSession = null;
        }

        private void OnMessageError(MessageErrorCode code)
        {
            LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::OnMessageError->message error.code = {code}");
            Dispose();
        }

        private void OnMessageReceived(int messageID, byte[] datas)
        {
            object message = messageParser.DecodeMessage(messageID, datas);

            foreach (var kvp in messageHandlerDic)
            {
                if(kvp.Value.OnMessageHanlder(messageID, message))
                {
                    return;
                }
            }
            LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::OnMessageReceived->the handler not found.messageID = {messageID}");
        }

        #region Send Data
        public void SendMessage(int messageID)
        {
            if (IsConnected())
            {
                byte[] netBytes = messageWriter.EncodeMessage(messageID);
                if (netBytes != null && netBytes.Length > 0)
                {
                    netSession.Send(netBytes);
                }
            }
        }

        public void SendMessage(int messageID,object message)
        {
            if(IsConnected())
            {
                byte[] messageBytes = messageParser.EncodeMessage(messageID, message);
                byte[] netBytes = messageWriter.EncodeMessage(messageID, messageBytes);
                if(netBytes!=null && netBytes.Length>0)
                {
                    netSession.Send(netBytes);
                }
            }

        }

        #endregion

        #region Register Message Handler

        public void RegisterMessageHandler(string name, IClientNetMessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(name))
            {
                handler.Net = this;
                messageHandlerDic.Add(name, handler);
            }
            else
            {
                LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::RegisterMessageHandler->the handler has been added.name={name}");
            }
        }

        public void UnregisterMessageHandler(string name)
        {
            if (messageHandlerDic.ContainsKey(name))
            {
                messageHandlerDic.Remove(name);
            }
            else
            {
                LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::UnregisterMessageHandler->The handler not found.name={name}");
            }
        }

        #endregion
    }
}
