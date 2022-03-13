using DotEngine.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.Notification
{
    public delegate void NotificationHandler(string name, object body);

    public class Dispatcher
    {
        private readonly Dictionary<string, List<NotificationHandler>> handlerDic = null;

        public Dispatcher()
        {
            handlerDic = new Dictionary<string, List<NotificationHandler>>();
        }

        public void Subscribe(IObserver observer)
        {
            string[] names = observer.ListInterestNotification();
            if (names == null || names.Length == 0)
            {
                return;
            }
            foreach (var name in names)
            {
                Subscribe(name, observer.HandleNotification);
            }
        }

        public void Subscribe(string name, NotificationHandler handler)
        {
            if (!handlerDic.TryGetValue(name, out var handlers))
            {
                handlers = ListPool<NotificationHandler>.Get();
                handlerDic.Add(name, handlers);

                handlers.Add(handler);
            }
            else if (handlers.Contains(handler))
            {
                throw new Exception("the handler has been registered");
            }
            handlers.Add(handler);
        }

        public void Unsubscribe(IObserver observer)
        {
            string[] names = observer.ListInterestNotification();
            if (names == null || names.Length == 0)
            {
                return;
            }
            foreach (var name in names)
            {
                Unsubscribe(name, observer.HandleNotification);
            }
        }

        public void Unsubscribe(string name, NotificationHandler handler)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                handlers.Remove(handler);

                if (handlers.Count == 0)
                {
                    ListPool<NotificationHandler>.Release(handlers);
                    handlerDic.Remove(name);
                }
            }
        }

        public void Unsubscribe(string name)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                ListPool<NotificationHandler>.Release(handlers);
                handlerDic.Remove(name);
            }
        }

        public void Notify(string name, object body = null)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                var tempHandlers = handlers.ToArray();
                foreach(var handler in tempHandlers)
                {
                    handler(name, body);
                }
            }
        }

        public void ClearAll()
        {
            handlerDic.Clear();
        }
    }
}
