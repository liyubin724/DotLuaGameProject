namespace DotEngine.Notify
{
    public abstract class Observer : IObserver
    {
        public void OnActivate()
        {
            if(Dispatcher != null)
            {
                string[] names = ListNotificationInterests();
                if (names != null && names.Length > 0)
                {
                    foreach(var name in names)
                    {
                        Dispatcher.RegisterObserver(name, HandleNotification);
                    }
                }
            }
        }

        public abstract string[] ListNotificationInterests();
        public abstract void HandleNotification(string notificationName,object body);

        public void OnDeactivate()
        {
            if (Dispatcher != null)
            {
                string[] names = ListNotificationInterests();
                if (names != null && names.Length > 0)
                {
                    foreach (var name in names)
                    {
                        Dispatcher.UnregisterObserver(name, HandleNotification);
                    }
                }
            }
        }

        private DispatchManager Dispatcher => DispatchManager.GetInstance();
    }
}
