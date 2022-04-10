namespace DotEngine.Notification
{
    public interface INotifier
    {
        Dispatcher Dispatcher { get; set; }

        void SendMessage(string name, object body = null);
    }
}
