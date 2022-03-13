namespace DotEngine.Notification
{
    public interface INotifier
    {
        void SendNotification(string name, object body = null);
    }
}
