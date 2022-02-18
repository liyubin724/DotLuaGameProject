using System;
using System.Collections.Generic;

namespace DotEngine.Notify
{
    public delegate void NotificationHandler(string name, object body);

    public sealed class DispatchManager
    {
        private static DispatchManager manager = null;

        public static DispatchManager GetInstance()
        {
            return manager;
        }

        public static DispatchManager CreateMgr()
        {
            if(manager == null)
            {
                manager = new DispatchManager();
                manager.OnInitilize();
            }
            return manager;
        }

        public static void DestroyMgr()
        {
            if(manager!=null)
            {
                manager.OnDestroy();
                manager = null;
            }
        }

        private readonly Dictionary<string, List<NotificationHandler>> observerDic = null;

        private DispatchManager()
        {
            observerDic = new Dictionary<string, List<NotificationHandler>>();
        }

        public void OnInitilize()
        {
        }

        public void OnDestroy()
        {
            observerDic.Clear();
        }

        public void RegisterObserver(string notificationName, NotificationHandler notifyHanlder)
        {
            if(!observerDic.TryGetValue(notificationName,out var handlers))
            {
                handlers = new List<NotificationHandler>();
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
                handlers.Remove(notifyHandler);
            }
        }

        public void UnregisterObserver(string notificationName)
        {
            if(observerDic.ContainsKey(notificationName))
            {
                observerDic.Remove(notificationName);
            }
        }

        public void Notify(string notificationName,object body = null)
        {
            if (observerDic.TryGetValue(notificationName, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    handler(notificationName,body);
                }
            }
        }

        public void ClearAll()
        {
            observerDic.Clear();
        }
    }
}
