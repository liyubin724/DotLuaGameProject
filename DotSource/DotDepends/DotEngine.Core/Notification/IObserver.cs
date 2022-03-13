namespace DotEngine.Notification
{
    public interface IObserver
    {
        void RegisterNotifications(Dispatcher dispatcher);
        void UnregisterNotifiations();

        string[] ListInterestNotification();
        void HandleNotification(string name, object body = null);
    }
}
