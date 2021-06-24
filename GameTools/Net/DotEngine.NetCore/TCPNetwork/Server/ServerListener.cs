using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace DotEngine.NetCore.TCPNetwork
{

    public delegate void ServerMessageHandler(Guid id, int msgId, byte[] msgBytes);

    public class ServerListener : IDisposable
    {
        private static string LOG_TAG = "ServerListener";

        private Dictionary<int, ServerMessageHandler> messageHandlerDic = new Dictionary<int, ServerMessageHandler>();
        public ServerMessageHandler FinallyMessageHandler { get; set; }

        private Dictionary<string, ServerNetwork> networkDic = new Dictionary<string, ServerNetwork>();

        private bool disposed = false;

        public bool StartServer(string name, int port, IMessageEncoder encoder = null, IMessageDecoder decoder = null)
        {
            if (!networkDic.ContainsKey(name))
            {
                encoder = encoder ?? new DefaultMessageEncoder();
                decoder = decoder ?? new DefaultMessageDecoder();

                ServerNetwork network = new ServerNetwork(name, IPAddress.Parse("127.0.0.1"), port, encoder, decoder);
                if (network.Start())
                {
                    networkDic.Add(name, network);
                    return true;
                }
                else
                {
                    NetLogger.Error(LOG_TAG, $"The server which named({name}) is not started");
                    return false;
                }
            }
            return true;
        }

        public void StopServer(string name)
        {
            if (networkDic.TryGetValue(name, out var network))
            {
                network.Stop();

                networkDic.Remove(name);
            }
        }

        #region Message Handlers
        public void RegisterMessageHandler(int msgID, ServerMessageHandler handler)
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
                        var handler = (ServerMessageHandler)Delegate.CreateDelegate(typeof(ServerMessageHandler), methodInfo);
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

        public void MulticastMsg(int msgId, byte[] msgBytes)
        {
            if (networkDic.Count == 0)
            {
                return;
            }
            foreach (var kvp in networkDic)
            {
                if (kvp.Value.IsStarted)
                {
                    kvp.Value.Multicast(msgId, msgBytes);
                }
                else
                {
                    NetLogger.Warning(LOG_TAG, $"The server({kvp.Key}) is not started");
                }
            }
        }

        public void MulticastMsg(string name, int msgId, byte[] msgBytes)
        {
            if (networkDic.Count > 0)
            {
                return;
            }
            if (networkDic.TryGetValue(name, out var network))
            {
                if (network.IsStarted)
                {
                    network.Multicast(msgId, msgBytes);
                }
                else
                {
                    NetLogger.Warning(LOG_TAG, $"The server({name}) is not started");
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if (networkDic.Count > 0)
            {
                return;
            }

            foreach (var kvp in networkDic)
            {
                ServerMessageData[] msgDatas = kvp.Value.ExtractMessageDatas();
                if (msgDatas != null && msgDatas.Length > 0)
                {
                    foreach (var data in msgDatas)
                    {
                        if (messageHandlerDic.TryGetValue(data.MsgId, out var handler))
                        {
                            handler.Invoke(data.Id, data.MsgId, data.MsgBytes);
                        }
                        else
                        {
                            FinallyMessageHandler?.Invoke(data.Id, data.MsgId, data.MsgBytes);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            Dispose();
        }

        ~ServerListener()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if(disposed)
            {
                return;
            }
            if(disposing)
            {
                foreach(var kvp in networkDic)
                {
                    kvp.Value.Dispose();
                }
            }

            networkDic.Clear();

            disposed = true;
        }
    }
}
