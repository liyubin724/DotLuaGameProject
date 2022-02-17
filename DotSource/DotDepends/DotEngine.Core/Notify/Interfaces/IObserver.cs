namespace DotEngine.Notify
{
    public interface IObserver
    {
        void OnActivate();
        string[] ListNotificationInterests();
        void HandleNotification(INotification notification);
        void OnDeactivate();
    }
}
