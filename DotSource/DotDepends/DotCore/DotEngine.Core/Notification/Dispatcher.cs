using DotEngine.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.Notification
{
    public delegate void MessageHandler(string name, object body);

    public class Dispatcher
    {
        private readonly Dictionary<string, List<MessageHandler>> observerDic = null;
        private readonly Stack<string> notifyNameStack = null;
        private readonly Dictionary<string, List<MessageHandler>> waitingToUnregisterObserverDic = null;

        public Dispatcher()
        {
            observerDic = new Dictionary<string, List<MessageHandler>>();
            notifyNameStack = new Stack<string>();
            waitingToUnregisterObserverDic = new Dictionary<string, List<MessageHandler>>();
        }

        public void RegisterObserver(IObserver observer)
        {
            string[] names = observer.ListInterestMessage();
            if(names!=null && names.Length>0)
            {
                for(int i =0;i<names.Length;++i)
                {
                    RegisterObserver(names[i], observer.HandleMessage);
                }
            }
        }

        public void RegisterObserver(string name, MessageHandler handler)
        {
            if (!observerDic.TryGetValue(name, out var handlers))
            {
                handlers = ListPool<MessageHandler>.Get();
                observerDic.Add(name, handlers);
            }

#if DEBUG
            if (handlers.Contains(handler))
            {
                throw new Exception("the handler has been registered");
            }
#endif
            handlers.Add(handler);
        }

        public void UnregisterObserver(IObserver observer)
        {
            string[] names = observer.ListInterestMessage();
            if (names != null && names.Length > 0)
            {
                for (int i = 0; i < names.Length; ++i)
                {
                    UnregisterObserver(names[i], observer.HandleMessage);
                }
            }
        }

        public void UnregisterObserver(string name, MessageHandler handler)
        {
            if (observerDic.TryGetValue(name, out var handlers))
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
                        ListPool<MessageHandler>.Release(handlers);
                        observerDic.Remove(name);
                    }
                }
            }
        }

        public void UnregisterObserver(string name)
        {
            if (observerDic.TryGetValue(name, out var handlers))
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
                    ListPool<MessageHandler>.Release(handlers);
                    observerDic.Remove(name);
                }
            }
        }

        public void Notify(string name, object body = null)
        {
            if (observerDic.TryGetValue(name, out var handlers))
            {
                notifyNameStack.Push(name);
                int index = 0;
                while (index < handlers.Count)
                {
                    var handler = handlers[index];
                    if (!waitingToUnregisterObserverDic.TryGetValue(name, out var unvalidHandlers))
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

        private void AddWaitingToUnregisterObserver(string notificationName, MessageHandler handler)
        {
            if (!waitingToUnregisterObserverDic.TryGetValue(notificationName, out var unregisterObserverList))
            {
                unregisterObserverList = ListPool<MessageHandler>.Get();
                waitingToUnregisterObserverDic.Add(notificationName, unregisterObserverList);
            }
            unregisterObserverList.Add(handler);
        }

        private void ClearWaitingUnregisterObserver()
        {
            foreach (var kvp in waitingToUnregisterObserverDic)
            {
                foreach (var handler in kvp.Value)
                {
                    UnregisterObserver(kvp.Key, handler);
                }
                ListPool<MessageHandler>.Release(kvp.Value);
            }
            waitingToUnregisterObserverDic.Clear();
        }
    }
}
