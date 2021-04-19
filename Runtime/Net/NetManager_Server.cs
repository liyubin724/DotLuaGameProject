using DotEngine.Net.Message;
using DotEngine.Net.Server;
using System.Collections.Generic;

namespace DotEngine.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private const int DEFAULT_MAX_CLIENT_COUNT = 100;
        private Dictionary<int, ServerNetListener> serverNetListenerDic = null;

        public ServerNetListener CreateServerNet(int netID,int listenPort, IMessageParser messageParser, int maxClientNetCount = DEFAULT_MAX_CLIENT_COUNT)
        {
            if (clientNetDic == null)
            {
                serverNetListenerDic = new Dictionary<int, ServerNetListener>();
            }
            if (serverNetListenerDic.ContainsKey(netID))
            {
                DebugLog.Error(NetConst.SERVER_LOGGER_TAG, $"NetMananger::CreateServerNet->the net has been created.netID={netID}");
                return null;
            }

            ServerNetListener serverNet = new ServerNetListener(netID,messageParser);
            serverNet.Startup("127.0.0.1", listenPort, maxClientNetCount);

            serverNetListenerDic.Add(netID, serverNet);

            return serverNet;
        }

        public ServerNetListener GetServerNet(int netID)
        {
            if (serverNetListenerDic != null && serverNetListenerDic.TryGetValue(netID, out ServerNetListener net))
            {
                return net;
            }
            return null;
        }

        public bool HasServerNet(int netID)
        {
            return serverNetListenerDic != null && serverNetListenerDic.ContainsKey(netID);
        }

        public void DestroyServerNet(int netID)
        {
            if (serverNetListenerDic.TryGetValue(netID, out ServerNetListener net))
            {
                net.Dispose();
                serverNetListenerDic.Remove(netID);
            }
        }

        public void DoUpdate_Server(float deltaTime)
        {
            if(serverNetListenerDic!=null && serverNetListenerDic.Count>0)
            {
                foreach(var kvp in serverNetListenerDic)
                {
                    kvp.Value.DoUpdate(deltaTime);
                }
            }
        }

        public void DoLateUpdate_Server(float deltaTime)
        {
            if (serverNetListenerDic != null && serverNetListenerDic.Count > 0)
            {
                foreach (var kvp in serverNetListenerDic)
                {
                    kvp.Value.DoLateUpdate();
                }
            }
        }

        private void DoDispose_Server()
        {
            if(serverNetListenerDic!=null)
            {
                foreach (var kvp in serverNetListenerDic)
                {
                    kvp.Value.Dispose();
                }

                serverNetListenerDic.Clear();
                serverNetListenerDic = null;
            }
        }
    }
}
