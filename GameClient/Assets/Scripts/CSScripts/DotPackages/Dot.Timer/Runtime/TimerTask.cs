using UnityEngine;

namespace DotEngine.Timer
{
    public enum TimerTaskCategory
    {
        None = 0,
        Interval,
        End,
        IntervalAndEnd,
    }

    public class TimerTask
    {
        internal int Index { get; private set; } = -1;

        private int m_IntervalInMS = 0;
        private int m_TotalInMS = 0;
        private TimerEventHandler m_OnIntervalEvent = null;
        private TimerEventHandler m_OnEndEvent = null;
        private object m_UserData = null;
        private TimerTaskCategory m_Category = TimerTaskCategory.None;

        internal int TriggerLeftInMS { get; set; } = 0;

        public TimerTask()
        {
        }

        internal void SetData(
            int index,
            float intervalInSec,
            float totalInSec,
            TimerEventHandler intervalCallback,
            TimerEventHandler endCallback,
            object userdata)
        {
            Index = index;

            if(intervalInSec <=0 && totalInSec > 0)
            {
                m_Category = TimerTaskCategory.End;
                m_TotalInMS = Mathf.RoundToInt(totalInSec * 1000);

                TriggerLeftInMS = m_TotalInMS;
            }else if(intervalInSec > 0 && totalInSec <= 0)
            {
                m_Category = TimerTaskCategory.Interval;
                m_IntervalInMS = Mathf.RoundToInt(intervalInSec * 1000);

                TriggerLeftInMS = m_IntervalInMS;
            }else if(intervalInSec>0 && totalInSec > 0)
            {
                m_Category = TimerTaskCategory.IntervalAndEnd;
                m_IntervalInMS = Mathf.RoundToInt(intervalInSec * 1000);
                m_TotalInMS = Mathf.RoundToInt(totalInSec * 1000);

                TriggerLeftInMS = m_IntervalInMS;
            }else
            {
                DebugLog.Error("Timer error");
            }
            m_OnIntervalEvent = intervalCallback;
            m_OnEndEvent = endCallback;
            m_UserData = userdata;
        }

        internal bool Trigger()
        {
            if(TriggerLeftInMS <= 0)
            {
                if(m_Category == TimerTaskCategory.End)
                {
                    m_OnEndEvent?.Invoke(m_UserData);
                    return true;
                }else if(m_Category == TimerTaskCategory.Interval)
                {
                    m_OnIntervalEvent?.Invoke(m_UserData);
                    TriggerLeftInMS = m_IntervalInMS;
                    return false;
                }else if(m_Category == TimerTaskCategory.IntervalAndEnd)
                {
                    m_OnIntervalEvent?.Invoke(m_UserData);
                    m_TotalInMS -= m_IntervalInMS;
                    if(m_TotalInMS <= 0)
                    {
                        m_OnEndEvent?.Invoke(m_UserData);
                        return true;
                    }else
                    {
                        if(m_TotalInMS >= m_IntervalInMS)
                        {
                            TriggerLeftInMS = m_IntervalInMS;
                            return false;
                        }else
                        {
                            m_Category = TimerTaskCategory.End;
                            TriggerLeftInMS = m_TotalInMS;
                            return false;
                        }
                    }
                }else
                {
                    DebugLog.Error("Timer error");
                    return true;
                }
            }else
            {
                return false;
            }
        }

        public void OnRelease()
        {
            Index = -1;
            m_IntervalInMS = 0;
            m_TotalInMS = 0;
            m_OnEndEvent = null;
            m_OnIntervalEvent = null;
            m_UserData = null;
            m_Category = TimerTaskCategory.None;
            TriggerLeftInMS = 0;
        }
    }
}
