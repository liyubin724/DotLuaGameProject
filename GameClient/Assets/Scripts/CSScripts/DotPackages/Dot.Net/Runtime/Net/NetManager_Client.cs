using DotEngine.Net.Client;
using DotEngine.Net.Message;
using System.Collections.Generic;

namespace DotEngine.Net
{
    public partial class NetManager : Singleton<NetManager>
    {
        private Dictionary<int, ClientNet> clientNetDic = null;

        public ClientNet CreateClientNet(int netID,IMessageParser messageParser)
        {
            if(clientNetDic == null)
            {
                clientNetDic = new Dictionary<int, ClientNet>();
            }
            if (clientNetDic.ContainsKey(netID))
            {
                DebugLog.Error(NetConst.CLIENT_LOGGER_TAG, $"NetMananger::CreateClientNet->the net has been created.netID={netID}");
                return null;
            }

            ClientNet net = new ClientNet(netID, messageParser);
            clientNetDic.Add(netID, net);

            return net;
        }

        public ClientNet GetClientNet(int netID)
        {
            if(clientNetDic!=null && clientNetDic.TryGetValue(netID,out ClientNet net))
            {
                return net;
            }
            return null;
        }

        public bool HasClientNet(int netID)
        {
            return clientNetDic != null && clientNetDic.ContainsKey(netID);
        }

        public void DestroyClientNet(int netID)
        {
            if(clientNetDic.TryGetValue(netID,out ClientNet net))
            {
                net.Dispose();
                clientNetDic.Remove(netID);
            }
        }

        public void DoUpdate_Client(float deltaTime)
        {
            if(clientNetDic!=null)
            {
                foreach(var kvp in clientNetDic)
                {
                    kvp.Value.DoUpdate(deltaTime);
                }
            }
        }

        public void DoLateUpdate_Client(float deltaTime)
        {
            if (clientNetDic != null)
            {
                foreach (var kvp in clientNetDic)
                {
                    kvp.Value.DoLateUpdate();
                }
            }
        }

        private void DoDispose_Client()
        {
            if (clientNetDic != null)
            {
                foreach (var kvp in clientNetDic)
                {
                    kvp.Value.Dispose();
                }
                clientNetDic.Clear();
                clientNetDic = null;
            }
        }
    }
}
