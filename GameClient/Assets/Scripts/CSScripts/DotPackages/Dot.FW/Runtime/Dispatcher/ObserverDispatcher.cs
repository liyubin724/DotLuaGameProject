using System.Collections.Generic;

namespace DotEngine.Framework.Dispatcher
{
    public class ObserverDispatcher :IObserverDispatcher
    {
        private readonly Dictionary<string, List<NotificationHandler>> handlerDic = new Dictionary<string, List<NotificationHandler>>();

        public void Initialized()
        {
        }

        public void Disposed()
        {
            handlerDic.Clear();
        }

        public void Notify(string name, object data)
        {
            if(handlerDic.TryGetValue(name,out var handlers))
            {
                foreach (var handler in handlers)
                {
                    handler?.Invoke(name, data);
                }
            }
        }

        public bool Contains(string name,NotificationHandler handler)
        {
            return handlerDic.ContainsKey(name) && (handlerDic[name].Contains(handler));
        }

        public void Register(string name, NotificationHandler handler)
        {
            if(!handlerDic.TryGetValue(name,out var list))
            {
                list = new List<NotificationHandler>();
                handlerDic.Add(name,list);
            }
            list.Add(handler);
        }

        public void Unregister(string name,NotificationHandler handler)
        {
            if (handlerDic.TryGetValue(name, out var handlers))
            {
                handlers.Remove(handler);
            }
        }

        public void Unregister(string name)
        {
            if(handlerDic.ContainsKey(name))
            {
                handlerDic.Remove(name);
            }
        }
    }
}
