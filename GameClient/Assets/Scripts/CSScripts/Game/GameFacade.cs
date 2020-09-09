using DotEngine;
using DotEngine.Net.Services;
using DotEngine.PMonitor;
using DotEngine.PMonitor.Sampler;

namespace Game
{
    public class GameFacade : Facade
    {
        public new static Facade GetInstance()
        {
            if(instance == null)
            {
                instance = new GameFacade();
            }
            return instance;
        }

        protected override void InitializeFacade()
        {
            base.InitializeFacade();
        }

        protected override void InitializeService()
        {
            base.InitializeService();

            ServerNetService serverNetService = new ServerNetService();
            service.RegisterServicer(serverNetService);

            ClientNetService clientNetService = new ClientNetService();
            service.RegisterServicer(clientNetService);
        }
    }
}
