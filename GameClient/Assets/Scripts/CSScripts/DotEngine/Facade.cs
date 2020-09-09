using DotEngine.Asset;
using DotEngine.GOPool;
using DotEngine.Lua;
using DotEngine.Services;
using DotEngine.Timer;
using DotEngine.Utilities;

namespace DotEngine
{
    public class Facade
    {
        protected static Facade instance = null;

        public static Facade GetInstance()
        {
            return instance;
        }

        protected ServiceCenter serviceCenter = null;
        protected Facade()
        {
            instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            DontDestroyHandler.CreateComponent<FacadeBehaviour>("Facade Behaviour");

            InitializeService();
        }

        protected virtual void InitializeService()
        {
            serviceCenter = new ServiceCenter();

            LuaEnvService luaEnvService = new LuaEnvService();
            serviceCenter.RegisterService(luaEnvService);

            TimerService timerService = new TimerService();
            serviceCenter.RegisterService(timerService);

            AssetService assetService = new AssetService();
            serviceCenter.RegisterService(assetService);

            GameObjectPoolService poolService = new GameObjectPoolService(assetService.InstantiateAsset);
            serviceCenter.RegisterService(poolService);
        }

        public virtual void RegisterService(IService service)
        {
            serviceCenter.RegisterService(service);
        }

        public virtual IService RetrieveService(string name)
        {
            return serviceCenter.RetrieveService(name);
        }

        public virtual T GetService<T>(string name) where T : IService
        {
            return (T)serviceCenter.RetrieveService(name);
        }

        public virtual void RemoveService(string name)
        {
            serviceCenter.RemoveService(name);
        }

        public virtual bool HasService(string name)
        {
            return serviceCenter.HasService(name);
        }

        public void Dispose()
        {
            serviceCenter.ClearService();
            instance = null;
        }

        internal void DoUpdate(float deltaTime)
        {
            serviceCenter.DoUpdate(deltaTime);
        }

        internal void DoUnscaleUpdate(float deltaTime)
        {
            serviceCenter.DoUnscaleUpdate(deltaTime);
        }

        internal void DoLateUpdate(float deltaTime)
        {
            serviceCenter.DoLateUpdate(deltaTime);
        }

        internal void DoFixedUpdate(float deltaTime)
        {
            serviceCenter.DoFixedUpdate(deltaTime);
        }
    }
}
