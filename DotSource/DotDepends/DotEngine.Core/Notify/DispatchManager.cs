using DotEngine.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.Notify
{
    public delegate void NotificationHandler(string name, object body);

    public class DispatchManager : Singleton<DispatchManager>
    {
        private readonly Dictionary<string, List<NotificationHandler>> observerDic = null;
        private readonly Stack<string> notifyNameStack = null;
        private readonly Dictionary<string, List<NotificationHandler>> waitingToUnregisterObserverDic = null;

        public DispatchManager()
        {
            observerDic = new Dictionary<string, List<NotificationHandler>>();
            notifyNameStack = new Stack<string>();
            waitingToUnregisterObserverDic = new Dictionary<string, List<NotificationHandler>>();
        }

        protected override void OnInit()
        {

        }

        protected override void OnDestroy()
        {
            observerDic.Clear();

            base.OnDestroy();
        }

        public void RegisterObserver(string notificationName, NotificationHandler notifyHanlder)
        {
            if (!observerDic.TryGetValue(notificationName, out var handlers))
            {
                handlers = ListPool<NotificationHandler>.Get();
                observerDic.Add(notificationName, handlers);
            }

#if DEBUG
            if (handlers.Contains(notifyHanlder))
            {
                throw new Exception("the handler has been registered");
            }
#endif
            handlers.Add(notifyHanlder);
        }

        public void UnregisterObserver(string notificationName, NotificationHandler notifyHandler)
        {
            if (observerDic.TryGetValue(notificationName, out var handlers))
            {
                if (IsDispatching(notificationName))
                {
                    AddWaitingToUnregisterObserver(notificationName, notifyHandler);
                }
                else
                {
                    handlers.Remove(notifyHandler);

                    if (handlers.Count == 0)
                    {
                        ListPool<NotificationHandler>.Release(handlers);
                        observerDic.Remove(notificationName);
                    }
                }
            }
        }

        public void UnregisterObserver(string notificationName)
        {
            if (observerDic.TryGetValue(notificationName, out var handlers))
            {
                if (IsDispatching(notificationName))
                {
                    foreach (var handler in handlers)
                    {
                        AddWaitingToUnregisterObserver(notificationName, handler);
                    }
                }
                else
                {
                    ListPool<NotificationHandler>.Release(handlers);
                    observerDic.Remove(notificationName);
                }
            }
        }

        public void Notify(string notificationName, object body = null)
        {
            if (observerDic.TryGetValue(notificationName, out var handlers))
            {
                notifyNameStack.Push(notificationName);
                int index = 0;
                while (index < handlers.Count)
                {
                    var handler = handlers[index];
                    if (!waitingToUnregisterObserverDic.TryGetValue(notificationName, out var unvalidHandlers))
                    {
                        handler(notificationName, body);
                    }
                    else if (!unvalidHandlers.Contains(handler))
                    {
                        handler(notificationName, body);
                    }

                    ++index;
                }
                notifyNameStack.Pop();
            }

            if (!IsDispatching() && waitingToUnregisterObserverDic.Count > 0)
            {
                ClearWaitingUnregisterObserver();
            }
        }

        public void ClearAll()
        {
            observerDic.Clear();
        }

        private bool IsDispatching(string name)
        {
            return notifyNameStack.Contains(name);
        }

        private bool IsDispatching()
        {
            return notifyNameStack.Count > 0;
        }

        private void AddWaitingToUnregisterObserver(string notificationName, NotificationHandler notifyHandler)
        {
            if (!waitingToUnregisterObserverDic.TryGetValue(notificationName, out var unregisterObserverList))
            {
                unregisterObserverList = ListPool<NotificationHandler>.Get();
                waitingToUnregisterObserverDic.Add(notificationName, unregisterObserverList);
            }
            unregisterObserverList.Add(notifyHandler);
        }

        private void ClearWaitingUnregisterObserver()
        {
            foreach (var kvp in waitingToUnregisterObserverDic)
            {
                foreach (var handler in kvp.Value)
                {
                    UnregisterObserver(kvp.Key, handler);
                }
                ListPool<NotificationHandler>.Release(kvp.Value);
            }
            waitingToUnregisterObserverDic.Clear();
        }
    }
}
