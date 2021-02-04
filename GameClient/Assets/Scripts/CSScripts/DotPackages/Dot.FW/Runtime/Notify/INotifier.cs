namespace DotEngine.Framework.Notify
{
    public interface INotifier
    {
        void SendNotification(string name, object body);
    }
}
