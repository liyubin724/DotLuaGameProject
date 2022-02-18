namespace DotEngine.Notify
{
    public interface INotifier
    {
        void SendNotification(string name, object body = null);
    }
}
