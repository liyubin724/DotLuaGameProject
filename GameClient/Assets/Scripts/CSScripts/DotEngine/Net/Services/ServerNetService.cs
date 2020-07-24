using DotEngine.Net.Message;
using DotEngine.Net.Server;
using DotEngine.Services;

namespace DotEngine.Net.Services
{
    public class ServerNetService : Service, IUpdate, ILateUpdate
    {
        public const string NAME = "ServerNetService";
        private const int SERVER_NET_ID = 1;

        private ServerNetListener serverNetListener = null;
        public ServerNetService() : base(NAME)
        { }

        public bool IsServerRunning()
        {
            return serverNetListener != null;
        }

        public ServerNetListener CreateNet(IMessageParser messageParser,int port = 9999)
        {
            if(serverNetListener == null)
            {
                serverNetListener = NetManager.GetInstance().CreateServerNet(SERVER_NET_ID, port, messageParser);
            }else
            {

            }
            return serverNetListener;
        }

        public ServerNetListener GetNet()
        {
            return serverNetListener;
        }

        public void DiposeNet()
        {
            NetManager.GetInstance().DestroyServerNet(SERVER_NET_ID);
            serverNetListener = null;
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
