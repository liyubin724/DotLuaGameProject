using DotEngine.Utilities;
using DotEngine.Pool;
using System;

namespace DotEngine.Timer
{
    public class TimerTask : IObjectPoolItem
    {
        private long id = -1;
        public long ID { get => id; }
        
        private int intervalInMS = 0;
        private int totalInMS = 0;
        private Action<object> onIntervalEvent = null;
        private Action<object> onEndEvent = null;
        private object userData = null;

        internal int RemainingInMS { get; set; } = 0;
        private int leftInMS = 0;

        public TimerTask()
        {
        }

        public void SetData(long id, float intervalInSec,
                                                float totalInSec,
                                                Action<object> intervalCallback,
                                                Action<object> endCallback,
                                                object callbackData)
        {
            this.id = id;
            intervalInMS = MathUtil.CeilToInt(intervalInSec * 1000);
            if (totalInSec <= 0)
            {
                totalInMS = 0;
            }
            else
            {
                totalInMS = MathUtil.CeilToInt(totalInSec * 1000);
            }
            onIntervalEvent = intervalCallback;
            onEndEvent = endCallback;
            userData = callbackData;

            RemainingInMS = intervalInMS;
            leftInMS = totalInMS;
        }

        internal bool IsEnd()
        {
            if (intervalInMS <= 0)
            {
                return true;
            }
            if(totalInMS <= 0)
            {
                return false;
            }else
            {
                return leftInMS > 0;
            }
        }

        internal void ResetTask()
        {
            RemainingInMS = intervalInMS;
        }

        internal void OnTaskInterval()
        {
            leftInMS -= intervalInMS;
            onIntervalEvent?.Invoke(userData);
        }

        internal void OnTaskEnd()
        {
            onEndEvent?.Invoke(userData);
        }

        public void OnNew()
        {
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            id = -1;
            intervalInMS = 0; ;
            totalInMS = 0;
            RemainingInMS = 0;
            leftInMS = 0;
            onIntervalEvent = null;
            onEndEvent = null;
            userData = null;
        }
    }
}
