using DotEngine.Framework.Dispatcher;
using DotEngine.Framework.Proxies;
using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;

namespace DotEngine.Framework
{
    public class Facade : IFacade
    {
        protected IServiceDispatcher serviceDispatcher = null;
        protected IProxyDispatcher proxyDispatcher = null;
        protected ICommandDispatcher commandDispatcher = null;
        protected IObserverDispatcher observerDispatcher = null;

        protected static IFacade facade = null;
        public static IFacade GetInstance()
        {
            if(facade == null)
            {
                facade = new Facade();
            }
            return facade;
        }

        public virtual void Initialize()
        {
            UpdaterBehaviour updater = UpdaterBehaviour.GetUpdater();
            updater.UpdateAction += DoUpdate;
            updater.LateUpdateAction += DoLateUpdate;
            updater.FixedUpdateAction += DoFixedUpdate;
            
            InitializeServices();

            serviceDispatcher.Register(UpdateService.NAME, new UpdateService(UpdateService.NAME));
            serviceDispatcher.Register(LateUpdateService.NAME, new LateUpdateService(LateUpdateService.NAME));
            serviceDispatcher.Register(FixedUpdateService.NAME, new FixedUpdateService(FixedUpdateService.NAME));

            InitializeObservers();
            InitializeCommands();
            InitializeProxies();
        }

        public virtual void Dispose()
        {

        }

        protected virtual void InitializeServices()
        {
            serviceDispatcher = new ServiceDispatcher();
        }

        protected virtual void InitializeProxies()
        {
            proxyDispatcher = new ProxyDispatcher();
        }

        protected virtual void InitializeCommands()
        {
            commandDispatcher = new CommandDispatcher();
        }

        protected virtual void InitializeObservers()
        {
            observerDispatcher = new ObserverDispatcher();
        }

        #region Service
        public bool HasService(string serviceName)
        {
            return serviceDispatcher == null ? false : serviceDispatcher.Contains(serviceName);
        }

        public void RegisterService(IService service)
        {
            serviceDispatcher.Register(service.Name, service);
        }

        public void UnregisterService(string serviceName)
        {
            serviceDispatcher.Unregister(serviceName);
        }

        public T RetrieveService<T>(string serviceName) where T : IService
        {
            return (T)serviceDispatcher?.Retrieve(serviceName);
        }

        #endregion

        #region Proxy
        public bool HasProxy(string proxyName)
        {
            return proxyDispatcher == null ? false : proxyDispatcher.Contains(proxyName);
        }

        public void RegisterProxy(string proxyName, IProxy proxy)
        {
            proxyDispatcher?.Register(proxyName, proxy);
        }

        public void UnregisterProxy(string proxyName)
        {
            proxyDispatcher?.Unregister(proxyName);
        }

        public T RetrieveProxy<T>(string proxyName) where T : IProxy
        {
            return (T)proxyDispatcher?.Retrieve(proxyName);
        }

        #endregion

        #region Command
        public bool HasCommand(string name)
        {
            return commandDispatcher == null ? false : commandDispatcher.Contains(name);
        }

        public void RegisterCommand(string name, CommandCreater creater)
        {
            RegisterObserver(name, ExecuteCommand);

            commandDispatcher.Register(name, creater);
        }

        public void UnregisterCommand(string name)
        {
            UnregisterObserver(name, ExecuteCommand);
            commandDispatcher.Unregister(name);
        }

        private void ExecuteCommand(string name,object data)
        {
            commandDispatcher.Execute(name, data);
        }

        #endregion

        #region Observer
        public void RegisterObserver(string name, NotificationHandler handler)
        {
            observerDispatcher.Register(name, handler);
        }

        public void UnregisterObserver(string name, NotificationHandler handler)
        {
            observerDispatcher.Unregister(name, handler);
        }

        public void UnregisterObservers(string name)
        {
            observerDispatcher.Unregister(name);
        }
        #endregion

        #region Notify
        public void SendNotification(string name,object data)
        {
            observerDispatcher?.Notify(name, data);
        }
        #endregion

        #region Update
        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            serviceDispatcher?.DoUpdate(deltaTime, unscaleDeltaTime);
        }

        public void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            serviceDispatcher.DoLateUpdate(deltaTime, unscaleDeltaTime);
        }

        public void DoFixedUpdate(float deltaTime,float unscaleDeltaTime)
        {
            serviceDispatcher?.DoFixedUpdate(deltaTime,unscaleDeltaTime);
        }
        #endregion
    }
}
