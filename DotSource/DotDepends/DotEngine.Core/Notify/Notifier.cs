namespace DotEngine.Notify
{
    public class Notifier : INotifier
    {
        public void SendNotification(string name, object body = null)
        {
            Dispatcher?.Notify(name, body);
        }

        private DispatchManager Dispatcher => DispatchManager.GetInstance();
    }
}
