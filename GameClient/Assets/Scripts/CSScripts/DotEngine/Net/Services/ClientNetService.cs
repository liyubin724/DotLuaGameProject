using DotEngine.Net.Client;
using DotEngine.Net.Message;
using DotEngine.Services;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Net.Services
{
    public class ClientNetService : Service, IUpdate, ILateUpdate
    {
        public const string NAME = "ClientNetService";

        private Dictionary<int, ClientNet> clientNetDic = new Dictionary<int, ClientNet>();

        public ClientNetService() : base(NAME)
        {
        }

        public override void DoRemove()
        {
            int[] netIds = clientNetDic.Keys.ToArray();
            foreach(var id in netIds)
            {
                DisposeNet(id);
            }
            clientNetDic.Clear();
        }

        public ClientNet CreateNet(int netID, IMessageParser messageParser)
        {
            ClientNet clientNet = NetManager.GetInstance().CreateClientNet(netID, messageParser);
            clientNet.NetConnecting += HandleNetConnecting;
            clientNet.NetConnectedSuccess += HandleNetConnectedSuccess;
            clientNet.NetConnectedFailed += HandleNetConnectedFailed;
            clientNet.NetDisconnected += HandleNetDisconnected;

            clientNetDic.Add(netID, clientNet);
            return clientNet;
        }

        public ClientNet GetNet(int netID)
        {
            if(clientNetDic.TryGetValue(netID,out ClientNet net))
            {
                return net;
            }
            return null;
        }

        public void DisposeNet(int netID)
        {
            if (clientNetDic.TryGetValue(netID, out ClientNet clientNet))
            {
                clientNet.NetConnecting -= HandleNetConnecting;
                clientNet.NetConnectedSuccess -= HandleNetConnectedSuccess;
                clientNet.NetConnectedFailed -= HandleNetConnectedFailed;
                clientNet.NetDisconnected -= HandleNetDisconnected;

                clientNetDic.Remove(netID);

                NetManager.GetInstance().DestroyClientNet(netID);
            }
        }

        private void HandleNetConnecting(ClientNet net)
        {
            //CallAction<int>(NetNotification.CLIENT_NET_CONNECDTING, net.UniqueID);
        }

        private void HandleNetConnectedSuccess(ClientNet net)
        {
            //CallAction<int>(NetNotification.CLIENT_NET_CONNECTED_SUCCESS, net.UniqueID);
        }

        private void HandleNetConnectedFailed(ClientNet net)
        {
            //CallAction<int>(NetNotification.CLIENT_NET_CONNECTED_FAILED, net.UniqueID);
        }

        private void HandleNetDisconnected(ClientNet net)
        {
            //CallAction<int>(NetNotification.CLIENT_NET_DISCONNECTED, net.UniqueID);
        }

        public void DoLateUpdate(float deltaTime)
        {
            NetManager.GetInstance().DoLateUpdate_Client(deltaTime);
        }

        public void DoUpdate(float deltaTime)
        {
            NetManager.GetInstance().DoUpdate_Client(deltaTime);
        }
    }
}
