﻿namespace DotEngine.Notification
{
    public abstract class ANotifier : INotifier
    {
        public Dispatcher Dispatcher { get; set; }

        public void SendMessage(string name, object body = null)
        {
            Dispatcher?.Notify(name, body);
        }
    }
}
