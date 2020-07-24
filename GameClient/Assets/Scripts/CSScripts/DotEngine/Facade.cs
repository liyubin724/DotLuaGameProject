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
            if(instance == null)
            {
                return instance = new Facade();
            }
            return instance;
        }

        private ServiceCenter m_ServiceCenter = null;
        protected Facade()
        {
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            DontDestroyHandler.CreateComponent<FacadeBehaviour>("Facade Behaviour");

            InitializeService();
        }

        protected virtual void InitializeService()
        {
            m_ServiceCenter = new ServiceCenter();
        }

        public virtual void RegisterService(IService service)
        {
            m_ServiceCenter.RegisterService(service);

            TimerService timerService = new TimerService();
            m_ServiceCenter.RegisterService(timerService);

            LuaEnvService luaEnvService = new LuaEnvService();
            m_ServiceCenter.RegisterService(luaEnvService);

            AssetService assetService = new AssetService();
            m_ServiceCenter.RegisterService(assetService);

            GameObjectPoolService poolService = new GameObjectPoolService(assetService.InstantiateAsset);
            m_ServiceCenter.RegisterService(poolService);
        }

        public virtual IService RetrieveService(string name)
        {
            return m_ServiceCenter.RetrieveService(name);
        }

        public virtual T GetService<T>(string name) where T : IService
        {
            return (T)m_ServiceCenter.RetrieveService(name);
        }

        public virtual void RemoveService(string name)
        {
            m_ServiceCenter.RemoveService(name);
        }

        public virtual bool HasService(string name)
        {
            return m_ServiceCenter.HasService(name);
        }

        internal void DoUpdate(float deltaTime)
        {
            m_ServiceCenter.DoUpdate(deltaTime);
        }

        internal void DoLateUpdate(float deltaTime)
        {
            m_ServiceCenter.DoLateUpdate(deltaTime);
        }

        internal void DoFixedUpdate(float deltaTime)
        {
            m_ServiceCenter.DoFixedUpdate(deltaTime);
        }
    }
}
