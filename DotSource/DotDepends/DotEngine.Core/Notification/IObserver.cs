namespace DotEngine.Notification
{
    public interface IObserver
    {
        string[] ListInterestMessage();
        void HandleMessage(string name, object body = null);
    }
}
