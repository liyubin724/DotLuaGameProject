using DotEngine.NetCore.Pool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace DotEngine.NetCore.TCPNetwork
{
    public enum ClientNetworkState
    {
        Unreachable = 0,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected,
        Error,
    }

    public delegate void ClientMessageHandler(int msgID, byte[] msgBytes);

    internal class ClientMessageData
    {
        public int MsgId { get; set; } = -1;
        public byte[] MsgBytes { get; set; } = null;
    }

    public class ClientNetConnector : IDisposable, IClientNetworkHandler
    {
        private static readonly string LOG_TAG = "ClientNetConnector";

        private IMessageEncoder messageEncoder = null;
        private IMessageDecoder messageDecoder = null;

        private ClientNetwork network = null;

        private object stateLocker = new object();
        private List<ClientNetworkState> cachedNetStates = new List<ClientNetworkState>();
        private ClientNetworkState curState = ClientNetworkState.Unreachable;

        public ClientNetworkState CurrentState => curState;

        private Dictionary<int, ClientMessageHandler> messageHandlerDic = new Dictionary<int, ClientMessageHandler>();
        public ClientMessageHandler FinallyMessageHandler { get; set; }

        private ObjectPool<ClientMessageData> messageDataPool = null;
        private object messageLocker = new object();
        private NetMessageBuffer messageBuffer = new NetMessageBuffer();
        private List<ClientMessageData> messageDatas = new List<ClientMessageData>();

        public Action OnNetConnecting = null;
        public Action OnNetConnected = null;
        public Action OnNetDisconnecting = null;
        public Action OnNetDisconnected = null;
        public Action OnNetError = null;

        public int MaxCountInOneTime { get; set; } = -1;

        public bool IsConnected
        {
            get
            {
                return network != null && network.IsConnected;
            }
        }

        public ClientNetConnector(IMessageEncoder encoder = null, IMessageDecoder decoder = null)
        {
            messageEncoder = encoder ?? new DefaultMessageEncoder();
            messageDecoder = decoder ?? new DefaultMessageDecoder();

            messageDataPool = new ObjectPool<ClientMessageData>(
                () => new ClientMessageData(), 
                null, 
                (data) =>
                {
                    data.MsgId = -1;
                    data.MsgBytes = null;
                });
        }

        #region Message Handlers
        public void RegisterMessageHandler(int msgID, ClientMessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(msgID))
            {
                messageHandlerDic.Add(msgID, handler);
            }
            else
            {
                NetLogger.Error(LOG_TAG, $"The handler({msgID}) has been registed");
            }
        }

        public void RegisterMessageHandler(Type handlerType)
        {
            MethodInfo[] methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (methodInfos != null && methodInfos.Length > 0)
            {
                foreach (var methodInfo in methodInfos)
                {
                    var attr = methodInfo.GetCustomAttribute<MessageHandlerAttribute>();
                    if (attr != null)
                    {
                        var handler = (ClientMessageHandler)Delegate.CreateDelegate(typeof(ClientMessageHandler), methodInfo);
                        if (handler != null)
                        {
                            RegisterMessageHandler(attr.MsgID, handler);
                        }
                    }
                }
            }
        }

        public void UnregisterMessageHandler(int msgID)
        {
            if (messageHandlerDic.ContainsKey(msgID))
            {
                messageHandlerDic.Remove(msgID);
            }
        }

        public void UnregisterMessageHandler(Type handlerType)
        {
            MethodInfo[] methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (methodInfos != null && methodInfos.Length > 0)
            {
                foreach (var methodInfo in methodInfos)
                {
                    var attr = methodInfo.GetCustomAttribute<MessageHandlerAttribute>();
                    if (attr != null)
                    {
                        UnregisterMessageHandler(attr.MsgID);
                    }
                }
            }
        }
        #endregion

        #region Send Message
        public void Send(int msgID, byte[] dataBytes)
        {
            if (!IsConnected)
            {
                return;
            }
            byte[] datas = messageEncoder.EncodeMessage(msgID, dataBytes);
            network.SendAsync(datas);
        }
        #endregion

        #region Connect or Reconnect or Disconnect
        public bool Connect(IPAddress ip, int port)
        {
            if (network != null)
            {
                return false;
            }
            network = new ClientNetwork(ip, port);
            network.NetworkHandler = this;
            return network.ConnectAsync();
        }

        public bool Connect(string ip, int port)
        {
            return Connect(IPAddress.Parse(ip), port);
        }

        public bool Reconnect()
        {
            if (network != null)
            {
                return network.ReconnectAsync();
            }
            return false;
        }

        public bool Disconnect()
        {
            if (network != null)
            {
                return network.DisconnectAsync();
            }
            return false;
        }
        #endregion


        public void Dispose()
        {
            OnNetConnecting = null;
            OnNetConnected = null;
            OnNetDisconnecting = null;
            OnNetDisconnected = null;
            OnNetError = null;

            if(network!=null)
            {
                network.Disconnect();
                network = null;
            }
            ProcessReset();
        }

        public void OnDataReceived(byte[] buffer, long offset, long size)
        {
            lock (messageLocker)
            {
                messageBuffer.WriteBytes(buffer, (int)offset, (int)size);

                if (messageBuffer.Length > 0)
                {
                    byte[][] dataBytesArr = messageBuffer.ReadMessages();
                    if (dataBytesArr != null && dataBytesArr.Length > 0)
                    {
                        foreach(var dataBytes in dataBytesArr)
                        {
                            if (messageDecoder.DecodeMessage(dataBytes, out int msgID, out byte[] msgBytes))
                            {
                                ClientMessageData data = messageDataPool.Get();
                                data.MsgId = msgID;
                                data.MsgBytes = msgBytes;
                                messageDatas.Add(data);
                            }else
                            {
                                NetLogger.Error(LOG_TAG, "error");
                            }
                        }
                    }
                }
            }
        }

        public void OnStateChanged(ClientNetworkState state)
        {
            lock (stateLocker)
            {
                cachedNetStates.Add(state);
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if (network == null || !network.IsConnected)
            {
                return;
            }

            ProcessMessage();
            ProcessState();

            if (curState == ClientNetworkState.Disconnected)
            {
                ProcessReset();
            }
        }

        private void ProcessMessage()
        {
            lock (messageLocker)
            {
                int count = messageDatas.Count;
                if (count > 0)
                {
                    if (MaxCountInOneTime > 0)
                    {
                        count = Math.Min(count, MaxCountInOneTime);
                    }
                    for (int i = 0;i<count;++i)
                    {
                        ClientMessageData data = messageDatas[0];
                        messageDatas.RemoveAt(0);

                        if (messageHandlerDic.TryGetValue(data.MsgId, out var handler))
                        {
                            handler.Invoke(data.MsgId, data.MsgBytes);
                        }
                        else
                        {
                            NetLogger.Error(LOG_TAG, "Error");
                        }
                        messageDataPool.Release(data);
                    }
                }
            }

        }

        private void ProcessState()
        {
            ClientNetworkState[] states = null;
            lock (stateLocker)
            {
                if (cachedNetStates.Count > 0)
                {
                    states = cachedNetStates.ToArray();
                    cachedNetStates.Clear();
                }
            }
            if (states != null && states.Length > 0)
            {
                foreach (var state in states)
                {
                    if (state != curState)
                    {
                        if (state == ClientNetworkState.Connecting)
                        {
                            OnNetConnecting?.Invoke();
                        }
                        else
                        if (state == ClientNetworkState.Connected)
                        {
                            OnNetConnected?.Invoke();
                        }
                        else if (state == ClientNetworkState.Disconnecting)
                        {
                            OnNetDisconnecting?.Invoke();
                        }
                        else if (state == ClientNetworkState.Disconnected)
                        {
                            OnNetDisconnected?.Invoke();
                        }
                        else if (state == ClientNetworkState.Error)
                        {
                            OnNetError?.Invoke();
                        }

                        curState = state;
                    }
                }
            }
        }

        private void ProcessReset()
        {
            lock (stateLocker)
            {
                cachedNetStates.Clear();
            }
            curState = ClientNetworkState.Unreachable;
            lock (messageLocker)
            {
                messageBuffer.ResetStream();
                for(int i =0;i<messageDatas.Count;++i)
                {
                    ClientMessageData data = messageDatas[i];
                    messageDatas.RemoveAt(0);

                    messageDataPool.Release(data);
                }
            }
        }

    }
}
