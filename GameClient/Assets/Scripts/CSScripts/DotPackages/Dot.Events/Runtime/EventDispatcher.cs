using DotEngine.Pool;
using System.Collections.Generic;

using SystemObject = System.Object;

namespace DotEngine.Events
{
    public delegate void EventHandler(SystemObject sender, int eventID, params SystemObject[] values);

    public class EventDispatcher : IUpdate
    {
        private static ItemObjectPool<EventData> eventDataPool = null;

        private Dictionary<int, List<EventHandler>> eventHandlerDic = new Dictionary<int, List<EventHandler>>();
        private List<EventData> delayEvents = new List<EventData>();

        public EventDispatcher()
        {
            if (eventDataPool == null)
            {
                eventDataPool = new ItemObjectPool<EventData>();
            }
        }

        public void RegistEvent(int eventID, EventHandler handler)
        {
            if (!eventHandlerDic.TryGetValue(eventID, out List<EventHandler> handlerList))
            {
                handlerList = new List<EventHandler>();
                eventHandlerDic.Add(eventID, handlerList);
            }

            handlerList.Add(handler);
        }

        public void UnregistEvent(int eventID, EventHandler handler)
        {
            if (eventHandlerDic.TryGetValue(eventID, out List<EventHandler> handlerList))
            {
                if (handlerList != null)
                {
                    for (int i = handlerList.Count - 1; i >= 0; --i)
                    {
                        if (handlerList[i] == null)
                        {
                            handlerList.RemoveAt(i);
                            continue;
                        }
                        else if (handlerList[i] == handler)
                        {
                            handlerList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void TriggerEvent(SystemObject sender,int eventID,params SystemObject[] values)
        {
            TriggerEvent(sender, eventID, -1.0f, values);
        }

        public void TriggerEvent(int eventID,params SystemObject[] values)
        {
            TriggerEvent(null, eventID, -1.0f, values);
        }

        public void TriggerEvent(SystemObject sender, int eventID, float delayTime, params SystemObject[] values)
        {
            if (delayTime <= 0)
            {
                if (eventHandlerDic.TryGetValue(eventID, out List<EventHandler> handlerList))
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
                                handlerList[i](sender, eventID, values);
                            }
                        }
                    }
                }
            }
            else
            {
                EventData e = eventDataPool.Get();
                e.Sender = sender;
                e.EventID = eventID;
                e.DelayTime = delayTime;
                e.Values = values;
                delayEvents.Add(e);

                if(delayEvents.Count == 1)
                {
                    UpdateRunner.AddUpdate(this);
                }
            }
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            for (int i = delayEvents.Count - 1; i >= 0; --i)
            {
                EventData eventData = delayEvents[i];
                eventData.DelayTime -= unscaleDeltaTime;
                if (eventData.DelayTime <= 0)
                {
                    TriggerEvent(eventData.Sender, eventData.EventID, -1.0f, eventData.Values);
                    delayEvents.RemoveAt(i);

                    eventDataPool.Release(eventData);
                }
            }

            if(delayEvents.Count == 0)
            {
                UpdateRunner.RemoveUpdate(this);
            }
        }

        public void Dispose()
        {
            eventHandlerDic.Clear();

            if (delayEvents.Count > 0)
            {
                UpdateRunner.RemoveUpdate(this);
            }
            for (int i = delayEvents.Count - 1; i >= 0; --i)
            {
                eventDataPool.Release(delayEvents[i]);
            }
            delayEvents.Clear();
        }
    }
}
