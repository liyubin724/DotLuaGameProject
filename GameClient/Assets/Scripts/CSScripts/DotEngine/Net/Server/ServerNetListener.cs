using DotEngine.Generic;
using DotEngine.Log;
using DotEngine.Net.Message;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DotEngine.Net.Server
{
    public delegate void ServerMessageHandler(int netID, int messageID, object message);

    public class ServerNetMessageData : IObjectPoolItem
    {
        public int netID = -1;
        public int messageID = -1;
        public object message = null;

        public void OnGet()
        {
        }

        public void OnNew()
        {
            
        }

        public void OnRelease()
        {
            netID = -1;
            messageID = -1;
            message = null;
        }
    }

    public partial class ServerNetListener //: IDispose
    {
        private int uniqueID = -1;
        public int UniqueID { get => uniqueID; }

        private ManualResetEvent allDone = new ManualResetEvent(false);

        private ObjectPool<ServerNetMessageData> dataPool = new ObjectPool<ServerNetMessageData>();
        private object dataListLock = new object();
        private List<ServerNetMessageData> dataList = new List<ServerNetMessageData>();

        private Socket socket = null;
        private UniqueIntID clientIDCreator = new UniqueIntID();

        private object netDicLock = new object();
        private Dictionary<int, ServerNet> netDic = new Dictionary<int, ServerNet>();

        private Dictionary<int, ServerMessageHandler> messageHandlerDic = new Dictionary<int, ServerMessageHandler>();

        private IMessageParser messageParser = null;
        public ServerNetListener(int id,IMessageParser messageParser)
        {
            uniqueID = id;
            this.messageParser = messageParser;
        }

        public void Startup(string ip,int port,int maxCount)
        {
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            LogUtil.LogInfo(NetConst.SERVER_LOGGER_TAG, $"ServerNetListener::Startup->address = {ipAddress.ToString()}");

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(maxCount);

                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        allDone.Reset();
                        LogUtil.LogInfo("ServerNet", "Waiting for a connection...");

                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        allDone.WaitOne();
                    }
                });
                thread.Start();
            }
            catch (Exception e)
            {
                LogUtil.LogInfo("ServerNet", e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            int netID = clientIDCreator.NextID;
            ServerNet serverNet = new ServerNet(netID, handler,messageParser);
            serverNet.MessageReceived = OnMessageReceived;
            serverNet.NetDisconnected = OnNetDisconnected;

            lock(netDicLock)
            {
                netDic.Add(netID, serverNet);
            }
        }

        private void OnMessageReceived(int netID,int messageID,byte[] msgBytes)
        {
            if (messageHandlerDic.TryGetValue(messageID, out ServerMessageHandler handler))
            {
                object message = messageParser.DecodeMessage(messageID, msgBytes);
                handler.Invoke(netID, messageID, message);
            }
            else
            {
                LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetListener::OnMessageReceived->the handler not found.messageID = {messageID}");
            }
        }

        private void OnNetDisconnected(int id)
        {
            lock(netDicLock)
            {
                if(netDic.ContainsKey(id))
                {
                    netDic.Remove(id);
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {
        }

        public void DoLateUpdate()
        {
            lock(netDicLock)
            {
                foreach(var kvp in netDic)
                {
                    kvp.Value.DoLateUpdate();
                }
            }
        }

        public void SendMessage(int netID,int messageID)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendMessage(messageID);
            }
        }

        public void SendMessage(int netID,int messageID,object message)
        {
            if (netDic.TryGetValue(netID, out ServerNet serverNet))
            {
                serverNet.SendMessage(messageID, message);
            }
        }

        public void Dispose()
        {
            allDone.Set();
            dataPool.Clear();
            lock(dataListLock)
            {
                dataList.Clear();
            }
            lock(netDicLock)
            {
                foreach(var kvp in netDic)
                {
                    kvp.Value.Dispose();
                }
                netDic.Clear();
            }

            if (socket != null)
            {
                if (socket.Connected)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception e)
                    {
                        LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetListener::Disconnect->e = {e.Message}");
                    }
                    finally
                    {
                        socket.Close();
                        socket = null;
                    }
                }
                else
                {
                    socket.Close();
                    socket = null;
                }
            }
        }

        #region Register Message Handler

        public void RegisterMessageHandler(int messageID, ServerMessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Add(messageID, handler);
            }
            else
            {
                LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetListener::RegisterMessageHandler->the handler has been added.messageID={messageID}");
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
                LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetListener::UnregisterMessageHandler->The handler not found.messageID={messageID}");
            }
        }

        #endregion
    }
}
