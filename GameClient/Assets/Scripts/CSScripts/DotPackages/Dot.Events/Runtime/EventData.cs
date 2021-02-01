using DotEngine.Pool;
using SystemObject = System.Object;

namespace DotEngine.Events
{
    internal class EventData : IObjectPoolItem
    {
        internal SystemObject Sender { get; set; }
        internal int EventID { get; set; }
        internal float DelayTime { get; set; }
        internal SystemObject[] Values { get; set; }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            Sender = null;
            EventID = -1;
            DelayTime = 0.0f;
            Values = null;
        }
    }
}
