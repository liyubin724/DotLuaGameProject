using DotEngine.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.Notification
{
    public delegate void NotificationHandler(string name, object body);

    public class Dispatcher
    {
        private readonly Dictionary<string, List<NotificationHandler>> handlerDic = null;

        private readonly Stack<string> notifyNameStack = null;
        private readonly Dictionary<string, List<NotificationHandler>> waitingUnsubscribeDic = null;

        public Dispatcher()
        {
            handlerDic = new Dictionary<string, List<NotificationHandler>>();
            notifyNameStack = new Stack<string>();
            waitingUnsubscribeDic = new Dictionary<string, List<NotificationHandler>>();
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

        public void Unsubscribe(string name, NotificationHandler handler)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                if (IsDispatching(name))
                {
                    AddWaitingToUnregisterObserver(name, handler);
                }
                else
                {
                    handlers.Remove(handler);

                    if (handlers.Count == 0)
                    {
                        ListPool<NotificationHandler>.Release(handlers);
                        handlerDic.Remove(name);
                    }
                }
            }
        }

        public void Unregister(string name)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                if (IsDispatching(name))
                {
                    foreach (var handler in handlers)
                    {
                        AddWaitingToUnregisterObserver(name, handler);
                    }
                }
                else
                {
                    ListPool<NotificationHandler>.Release(handlers);
                    handlerDic.Remove(name);
                }
            }
        }

        public void Notify(string name, object body = null)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                notifyNameStack.Push(name);
                int index = 0;
                while (index < handlers.Count)
                {
                    var handler = handlers[index];
                    if (!waitingUnsubscribeDic.TryGetValue(name, out var unvalidHandlers))
                    {
                        handler(name, body);
                    }
                    else if (!unvalidHandlers.Contains(handler))
                    {
                        handler(name, body);
                    }

                    ++index;
                }
                notifyNameStack.Pop();
            }

            if (!IsDispatching() && waitingUnsubscribeDic.Count > 0)
            {
                ClearWaitingUnregisterObserver();
            }
        }

        public void ClearAll()
        {
            handlerDic.Clear();
        }

        private bool IsDispatching(string name)
        {
            return notifyNameStack.Contains(name);
        }

        private bool IsDispatching()
        {
            return notifyNameStack.Count > 0;
        }

        private void AddWaitingToUnregisterObserver(string name, NotificationHandler handler)
        {
            if (!waitingUnsubscribeDic.TryGetValue(name, out var list))
            {
                list = ListPool<NotificationHandler>.Get();
                waitingUnsubscribeDic.Add(name, list);
            }
            list.Add(handler);
        }

        private void ClearWaitingUnregisterObserver()
        {
            foreach (var kvp in waitingUnsubscribeDic)
            {
                foreach (var handler in kvp.Value)
                {
                    Unsubscribe(kvp.Key, handler);
                }
                ListPool<NotificationHandler>.Release(kvp.Value);
            }
            waitingUnsubscribeDic.Clear();
        }
    }
}
