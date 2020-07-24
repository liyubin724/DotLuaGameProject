using DotEngine.Pool;
using DotEngine.Timer;
using System.Collections.Generic;

namespace DotEngine.EventDispatch
{
    public delegate void EventHandler(EventData e);

    public class EventDispatcher
    {
        private static ObjectPool<EventData> eventDataPool = null;
        private Dictionary<int, List<EventHandler>> eventHandlerDic = null;
        private Dictionary<EventData, TimerTaskHandler> delayEventTaskInfo = null;

        public EventDispatcher()
        {
            if (eventDataPool == null)
            {
                eventDataPool = new ObjectPool<EventData>();
            }

            eventHandlerDic = new Dictionary<int, List<EventHandler>>();
            delayEventTaskInfo = new Dictionary<EventData, TimerTaskHandler>();
        }

        public void DoReset()
        {
            foreach (var kvp in eventHandlerDic)
            {
                kvp.Value.Clear();
            }
            foreach (var kvp in delayEventTaskInfo)
            {
                TimerManager.GetInstance().RemoveTimer(kvp.Value);
                eventDataPool.Release(kvp.Key);
            }
            delayEventTaskInfo.Clear();
        }

        public void DoDispose()
        {
            DoReset();

            eventDataPool?.Clear();
            eventDataPool = null;
            eventHandlerDic = null;
            delayEventTaskInfo = null;
        }

        public void RegisterEvent(int eventID, EventHandler handler)
        {
            if (!eventHandlerDic.TryGetValue(eventID, out List<EventHandler> handlerList))
            {
                handlerList = new List<EventHandler>();
                eventHandlerDic.Add(eventID, handlerList);
            }

            handlerList.Add(handler);
        }

        public void UnregisterEvent(int eventID, EventHandler handler)
        {
            if (eventHandlerDic.TryGetValue(eventID, out List<EventHandler> handlerList))
            {
                if (handlerList != null)
                {
                    for (int i = handlerList.Count - 1; i >= 0; i--)
                    {
                        if (handlerList[i] == null || handlerList[i] == handler)
                        {
                            handlerList.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public void TriggerEvent(int eventID,params object[] datas)
        {
            TriggerEvent(eventID, 0.0f, datas);
        }

        public void TriggerEvent(int eventID, float delayTime, params object[] datas)
        {
            EventData e = eventDataPool.Get();
            e.SetData(eventID, delayTime, datas);

            if (e.EventDelayTime <= 0)
            {
                TriggerEvent(e);
            }
            else
            {
                TimerTaskHandler handler = TimerManager.GetInstance().AddEndTimer(delayTime, OnDelayEventTrigger, e);
                delayEventTaskInfo.Add(e, handler);
            }
        }

        private void OnDelayEventTrigger(object userdata)
        {
            if (userdata != null && userdata is EventData eData)
            {
                delayEventTaskInfo.Remove(eData);

                TriggerEvent(eData);
            }
        }

        private void TriggerEvent(EventData e)
        {
            if (eventHandlerDic.TryGetValue(e.EventID, out List<EventHandler> handlerList))
            {
                if (handlerList != null && handlerList.Count > 0)
                {
                    for (var i = handlerList.Count - 1; i >= 0; --i)
                    {
                        if (handlerList[i] == null)
                        {
                            handlerList.RemoveAt(i);
                        }
                        else
                        {
                            handlerList[i](e);
                        }
                    }
                    eventDataPool.Release(e);
                }
            }
        }
    }
}
