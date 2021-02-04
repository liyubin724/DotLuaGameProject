using DotEngine.Framework.Dispatcher;
using DotEngine.Framework.Proxies;
using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;

namespace DotEngine.Framework
{
    public interface IFacade : IUpdate, ILateUpdate, IFixedUpdate
    {
        void Initialize();
        void Dispose();

        bool HasService(string serviceName);
        void RegisterService(IService service);
        void UnregisterService(string serviceName);
        T RetrieveService<T>(string serviceName) where T : IService;

        bool HasProxy(string proxyName);
        void RegisterProxy(string proxyName, IProxy proxy);
        void UnregisterProxy(string proxyName);
        T RetrieveProxy<T>(string proxyName) where T : IProxy;

        bool HasCommand(string name);
        void RegisterCommand(string name, CommandCreater creater);
        void UnregisterCommand(string name);

        void RegisterObserver(string name, NotificationHandler handler);
        void UnregisterObserver(string name, NotificationHandler handler);
        void UnregisterObservers(string name);

        void SendNotification(string name, object data);
    }
}
