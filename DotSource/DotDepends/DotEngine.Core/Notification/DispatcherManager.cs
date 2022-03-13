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

        public void Subscribe(IObserver observer)
        {
            dispatcher.Subscribe(observer);
        }

        public void Subscribe(string name, NotificationHandler handler)
        {
            dispatcher.Subscribe(name, handler);
        }

        public void Unsubscribe(IObserver observer)
        {
            dispatcher.Unsubscribe(observer);
        }

        public void Unsubscribe(string name, NotificationHandler handler)
        {
            dispatcher.Unsubscribe(name, handler);
        }

        public void Unsubscribe(string name)
        {
            dispatcher.Unsubscribe(name);
        }

        public void Notify(string name, object body = null)
        {
            dispatcher.Notify(name, body);
        }
    }
}
