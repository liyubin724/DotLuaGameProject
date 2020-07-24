namespace DotEngine.EventDispatch
{
    public class EventManager : Singleton<EventManager>
    {
        private EventDispatcher eventDispatcher = null;

        protected override void DoInit()
        {
            eventDispatcher = new EventDispatcher();
        }

        public override void DoDispose()
        {
            eventDispatcher.DoDispose();
            eventDispatcher = null;
            base.DoDispose();
        }

        public void RegisterEvent(int eventID, EventHandler handler) => eventDispatcher.RegisterEvent(eventID, handler);
        public void UnregisterEvent(int eventID, EventHandler handler) => eventDispatcher.UnregisterEvent(eventID, handler);
        
        public void TriggerEvent(int eventID) => eventDispatcher.TriggerEvent(eventID, 0, null);
        public void TriggerEvent(int eventID, float delay) => eventDispatcher.TriggerEvent(eventID, delay, null);
        public void TriggerEvent(int eventID, params object[] datas) => eventDispatcher.TriggerEvent(eventID, 0, datas);
        public void TriggerEvent(int eventID, float delay, params object[] datas) => eventDispatcher.TriggerEvent(eventID, delay, datas);
    }
}
