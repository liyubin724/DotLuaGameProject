using DotEngine.Pool;

namespace DotEngine.Events
{
    public static class EventManager
    {
        private static GenericObjectPool<EventDispatcher> sm_DispatcherPool = null;

        static EventManager()
        {
            sm_DispatcherPool = new GenericObjectPool<EventDispatcher>(
                                () => new EventDispatcher(),
                                null,
                                (dispatcher) =>
                                {
                                    dispatcher.Dispose();
                                });
        }

        public static EventDispatcher GetDispatcher()
        {
            return sm_DispatcherPool.Get();
        }

        public static void ReleaseDispatcher(EventDispatcher dispatcher)
        {
            sm_DispatcherPool.Release(dispatcher);
        }

        private static EventDispatcher sm_GlobalDispatcher = null;
        public static EventDispatcher GlobalDispatcher
        {
            get
            {
                if(sm_GlobalDispatcher == null)
                {
                    sm_GlobalDispatcher = new EventDispatcher();
                }
                return sm_GlobalDispatcher;
            }
        }
    }
}
