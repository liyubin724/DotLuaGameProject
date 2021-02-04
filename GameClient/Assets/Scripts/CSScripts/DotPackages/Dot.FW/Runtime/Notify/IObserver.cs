namespace DotEngine.Framework.Notify
{
    public interface IObserver
    {
        void HandleNotification(string name, object body);
    }
}
