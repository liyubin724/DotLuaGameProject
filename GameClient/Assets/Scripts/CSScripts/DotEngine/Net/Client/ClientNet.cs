using DotEngine.Log;
using DotEngine.Net.Message;
using System.Collections.Generic;

namespace DotEngine.Net.Client
{
    public delegate void ClientNetStateChanged(ClientNet clientNet);

    public delegate void ClientMessageHandler(int messageID, object message);

    public delegate void ClientFinalMessageHandler(int messageID, byte[] msgDatas);

    public partial class ClientNet //: IDispose
    {
        private int uniqueID = -1;
        public int UniqueID { get => uniqueID; }

        private MessageWriter messageWriter = null;
        private MessageReader messageReader = null;
        private IMessageParser messageParser = null;

        private ClientNetSession netSession = null;
        private ClientNetSessionState sessionState = ClientNetSessionState.Unavailable;

        private Dictionary<int, ClientMessageHandler> messageHandlerDic = new Dictionary<int, ClientMessageHandler>();

        public ClientFinalMessageHandler FinalMessageHandler { get; set; } = null;

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
            if(!messageHandlerDic.TryGetValue(messageID, out ClientMessageHandler messageHandler))
            {
                if (FinalMessageHandler != null)
                {
                    LogUtil.LogWarning(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::OnMessageReceived->the handler not found.messageID = {messageID}");
                    FinalMessageHandler.Invoke(messageID, datas);
                }
                else
                {
                    LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::OnMessageReceived->the handler not found.messageID = {messageID}");
                }
            }else
            {
                object message = messageParser.DecodeMessage(messageID, datas);
                messageHandler.Invoke(messageID, message);
            }
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

        public void RegisterMessageHandler(int messageID, ClientMessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Add(messageID, handler);
            }
            else
            {
                LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::RegisterMessageHandler->the handler has been added.messageID={messageID}");
            }
        }

        public void UnregisterMessageHandler(int messageID)
        {
            if (messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Remove(messageID);
            }
            else
            {
                LogUtil.LogError(NetConst.CLIENT_LOGGER_TAG, $"ClientNet::UnregisterMessageHandler->The handler not found.messageID={messageID}");
            }
        }

        #endregion
    }
}
