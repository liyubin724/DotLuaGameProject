using DotEngine.Asset;
using DotEngine.GOPool;
using DotEngine.Lua;
using DotEngine.Services;
using DotEngine.Timer;
using DotEngine.Utilities;

namespace DotEngine
{
    public abstract class Facade
    {
        protected static Facade instance = null;

        public static Facade GetInstance()
        {
            return instance;
        }

        protected Service service = null;
        protected Facade()
        {
            instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            DontDestroyUtility.CreateComponent<FacadeBehaviour>("Facade Behaviour");

            InitializeService();
        }

        protected virtual void InitializeService()
        {
            service = new Service();

            LuaEnvService luaEnvService = new LuaEnvService();
            service.RegisterServicer(luaEnvService);

            TimerService timerService = new TimerService();
            service.RegisterServicer(timerService);

            AssetService assetService = new AssetService();
            service.RegisterServicer(assetService);

            GameObjectPoolService poolService = new GameObjectPoolService(assetService.InstantiateAsset);
            service.RegisterServicer(poolService);
        }

        public virtual void RegisterServicer(IServicer servicer)
        {
            this.service.RegisterServicer(servicer);
        }

        public virtual IServicer RetrieveServicer(string name)
        {
            return service.RetrieveServicer(name);
        }

        public virtual T GetServicer<T>(string name) where T : IServicer
        {
            return (T)service.RetrieveServicer(name);
        }

        public virtual void RemoveServicer(string name)
        {
            service.RemoveServicer(name);
        }

        public virtual bool HasServicer(string name)
        {
            return service.HasServicer(name);
        }

        public void Dispose()
        {
            service.ClearServicer();
            instance = null;
        }

        internal void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            service.DoUpdate(deltaTime,unscaleDeltaTime);
        }

        internal void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            service.DoLateUpdate(deltaTime, unscaleDeltaTime);
        }

        internal void DoFixedUpdate(float deltaTime, float unscaleDeltaTime)
        {
            service.DoFixedUpdate(deltaTime, unscaleDeltaTime);
        }
    }
}
