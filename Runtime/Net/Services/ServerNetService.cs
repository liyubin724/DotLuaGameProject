using DotEngine.Net.Message;
using DotEngine.Net.Server;
using DotEngine.Services;

namespace DotEngine.Net.Services
{
    public class ServerNetService : Servicer, IUpdate, ILateUpdate
    {
        public const string NAME = "ServerNetService";

        public ServerNetService() : base(NAME)
        { }

        public ServerNetListener CreateNet(int serverID,IMessageParser messageParser,int port = 9999)
        {
            ServerNetListener serverNetListener = NetManager.GetInstance().GetServerNet(serverID);
            if(serverNetListener == null)
            {
                serverNetListener = NetManager.GetInstance().CreateServerNet(serverID, port, messageParser);
            }

            return serverNetListener;
        }

        public ServerNetListener GetNet(int serverID)
        {
            return NetManager.GetInstance().GetServerNet(serverID);
        }

        public void DiposeNet(int serverID)
        {
            NetManager.GetInstance().DestroyServerNet(serverID);
        }

        public void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            NetManager.GetInstance().DoLateUpdate_Server(deltaTime);
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            NetManager.GetInstance().DoUpdate_Server(deltaTime);
        }
    }
}
