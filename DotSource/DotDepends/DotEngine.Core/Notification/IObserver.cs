namespace DotEngine.Notification
{
    public interface IObserver
    {
        string[] ListInterestNotification();
        void HandleNotification(string name, object body = null);
    }
}
