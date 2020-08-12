using DotEngine.Net.Message;
using DotEngine.Net.Server;
using DotEngine.Services;

namespace DotEngine.Net.Services
{
    public class ServerNetService : Service, IUpdate, ILateUpdate
    {
        public const string NAME = "ServerNetService";

        public ServerNetService() : base(NAME)
        { }

        public ServerNetListener CreateNet(int netID,IMessageParser messageParser,int port = 9999)
        {
            ServerNetListener serverNetListener = NetManager.GetInstance().GetServerNet(netID);
            if(serverNetListener == null)
            {
                serverNetListener = NetManager.GetInstance().CreateServerNet(netID, port, messageParser);
            }

            return serverNetListener;
        }

        public ServerNetListener GetNet(int netID)
        {
            return NetManager.GetInstance().GetServerNet(netID);
        }

        public void DiposeNet(int netID)
        {
            NetManager.GetInstance().DestroyServerNet(netID);
        }

        public void DoLateUpdate(float deltaTime)
        {
            NetManager.GetInstance().DoLateUpdate_Server(deltaTime);
        }

        public void DoUpdate(float deltaTime)
        {
            NetManager.GetInstance().DoUpdate_Server(deltaTime);
        }
    }
}
