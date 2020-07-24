using DotEngine.Pool;
using SystemObject = System.Object;

namespace DotEngine.EventDispatch
{
    public class EventData : IObjectPoolItem
    {
        private int eventID = -1;
        private float eventDelayTime = 0.0f;
        private SystemObject[] eventParams = null;

        public int EventID => eventID;
        public float EventDelayTime => eventDelayTime;
        public int ParamCount => eventParams == null ? 0 : eventParams.Length;
        public SystemObject[] EventParams => eventParams;

        public EventData() { }

        internal void SetData(int eID, float eDelayTime, params object[] objs)
        {
            eventID = eID;
            eventDelayTime = eDelayTime;
            eventParams = objs;
        }

        public T GetValue<T>(int index = 0)
        {
            object result = GetObjectValue(index);
            if (result == null)
                return default;
            else
                return (T)result;
        }

        public object GetObjectValue(int index = 0)
        {
            if (eventParams == null || eventParams.Length == 0)
            {
                return null;
            }
            if (index < 0 || index >= eventParams.Length)
            {
                return null;
            }

            return eventParams[index];
        }

        public void OnGet()
        {

        }

        public void OnRelease()
        {
            eventID = -1;
            eventDelayTime = 0.0f;
            eventParams = null;
        }

        public void OnNew()
        {
        }
    }
}
