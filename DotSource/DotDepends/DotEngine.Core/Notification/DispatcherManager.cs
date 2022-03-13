namespace DotEngine.Notification
{
    public class DispatcherManager : Singleton<DispatcherManager>
    {
        private Dispatcher dispatcher = new Dispatcher();
        public DispatcherManager()
        {
        }

        protected override void OnInit()
        {
        }

        protected override void OnDestroy()
        {
            dispatcher.ClearAll();
            base.OnDestroy();
        }

        public void RegisterObserver(IObserver observer)
        {
            dispatcher.RegisterObserver(observer);
        }

        public void RegisterObserver(string name, NotificationHandler handler)
        {
            dispatcher.Subscribe(name, handler);
        }

        public void UnregisterObserver(IObserver observer)
        {
            dispatcher.UnregisterObserver(observer);
        }

        public void UnregisterObserver(string name, NotificationHandler handler)
        {
            dispatcher.Unsubscribe(name, handler);
        }

        public void UnregisterObserver(string name)
        {
            dispatcher.Unregister(name);
        }

        public void Notify(string name, object body = null)
        {
            dispatcher.Notify(name, body);
        }
    }
}
