namespace DotEngine.Notify
{
    public interface IObserver
    {
        void OnActivate();
        string[] ListNotificationInterests();
        void HandleNotification(string notificationName, object body);
        void OnDeactivate();
    }
}
