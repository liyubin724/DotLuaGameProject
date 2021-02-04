using SystemObject = System.Object;

namespace DotEngine.Framework.Dispatcher
{
    public delegate void NotificationHandler(string name, SystemObject data);

    public interface IObserverDispatcher
    {
        void Initialized();
        void Disposed();

        bool Contains(string name, NotificationHandler handler);
        void Register(string name, NotificationHandler handler);
        void Unregister(string name, NotificationHandler handler);
        void Unregister(string name);

        void Notify(string name, SystemObject data);
    }
}
