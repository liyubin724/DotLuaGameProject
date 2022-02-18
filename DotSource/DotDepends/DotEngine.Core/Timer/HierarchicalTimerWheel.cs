using DotEngine.Generic;
using DotEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Timer
{
    public delegate void TimerEventHandler(object userdata);

    /// <summary>
    /// 多层时间轮
    /// </summary>
    public class HierarchicalTimerWheel
    {
        private GenericObjectPool<TimerTask> m_TaskPool = null;
        private UniqueIntIDCreator m_IndexCreator = new UniqueIntIDCreator();

        private TimerWheel m_LowLevelWheel = null;
        private TimerWheel[] m_Wheels = null;

        private bool m_IsPaused = false;

        private Dictionary<int, TimerInstance> m_Handlers = new Dictionary<int, TimerInstance>();
        private float m_ElapseInMS = 0.0f;

        public static HierarchicalTimerWheel CreateDefaultHTW()
        {
            TimerWheel[] wheels = new TimerWheel[] 
            {
                new TimerWheel(0, 100, 10),
                new TimerWheel(1, 100 * 10, 60),
                new TimerWheel(2, 100 * 10 * 60, 60),
                new TimerWheel(3, 100 * 10 * 60 * 60, 24),
                new TimerWheel(4, 100 * 10 * 60 * 60 * 24, 30),
            };
            return new HierarchicalTimerWheel(wheels);
        }

        public HierarchicalTimerWheel(TimerWheel[] wheels) 
        {
            m_TaskPool = new GenericObjectPool<TimerTask>(() => new TimerTask(), null, (task) => task.Reset());

            if(wheels!=null && wheels.Length>0)
            {
                m_Wheels = wheels;
                for(int i =0;i<wheels.Length;++i)
                {
                    TimerWheel wheel = wheels[i];
                    wheel.Level = i;
                    wheel.completeEvent = OnCompleted;
                    wheel.slotTriggerEvent = OnSoltTrigger;
                }

                m_LowLevelWheel = wheels[0];
            }
        }

        public TimerInstance AddTimer(
            float intervalInSec,
            float totalInSec,
            TimerEventHandler intervalCallback,
            TimerEventHandler endCallback,
            object userdata)
        {
            int index = m_IndexCreator.NextID;

            TimerTask task = m_TaskPool.Get();
            task.SetData(index, intervalInSec, totalInSec, intervalCallback, endCallback, userdata);

            return AddTask(task, null);
        }

        public TimerInstance AddIntervalTimer(
            float intervalInSec,
           TimerEventHandler intervalCallback,
            object userdata)
        {
            return AddTimer(intervalInSec, 0, intervalCallback, null, userdata);
        }

        public TimerInstance AddTickTimer(
            TimerEventHandler intervalCallback,
            object userdata
            )
        {
            return AddTimer(m_LowLevelWheel.TickInMS * 0.001f, 0, intervalCallback, null, userdata);
        }

        public TimerInstance AddEndTimer(
            float totalInSec,
            TimerEventHandler endCallback,
            object userdata)
        {
            return AddTimer(0, totalInSec, null, endCallback, userdata);
        }

        private TimerInstance AddTask(TimerTask task,TimerInstance handler)
        {
            int wheelIndex = -1;
            int slotIndex = -1;
            for (int i = 0; i < m_Wheels.Length; ++i)
            {
                TimerWheel wheel = m_Wheels[i];
                if (wheel.TotalTickInMS >= task.TriggerLeftInMS)
                {
                    slotIndex = wheel.AddTask(task);
                    wheelIndex = i;
                    break;
                }
            }
            if(handler!=null)
            {
                handler.WheelIndex = wheelIndex;
                handler.WheelSlotIndex = slotIndex;
            }else
            {
                handler = new TimerInstance()
                {
                    Index = task.Index,
                    WheelIndex = wheelIndex,
                    WheelSlotIndex = slotIndex,
                };
                m_Handlers.Add(task.Index, handler);
            }
            
            return handler;
        }

        public bool RemoveTimer(TimerInstance handler)
        {
            if(handler!=null && m_Handlers.ContainsKey(handler.Index))
            {
                m_Handlers.Remove(handler.Index);
                if (handler.IsValid())
                {
                    TimerTask task = m_Wheels[handler.WheelIndex].RemoveTask(handler);
                    handler.Clear();
                    if (task != null)
                    {
                        m_TaskPool.Release(task);
                        return true;
                    }
                }
            }

            return false;
        }

        public void DoUpdate(float deltaTime)
        {
            if(!m_IsPaused && m_Handlers.Count>0)
            {
                m_ElapseInMS += Mathf.RoundToInt(deltaTime * 1000);

                if (m_ElapseInMS >= m_LowLevelWheel.TickInMS)
                {
                    int count = Mathf.FloorToInt(m_ElapseInMS / m_LowLevelWheel.TickInMS);
                    m_LowLevelWheel.DoPushWheel(count);

                    m_ElapseInMS %= m_LowLevelWheel.TickInMS;
                }
            }
        }

        public void Pause()
        {
            m_IsPaused = true;
        }

        public void Resume()
        {
            m_IsPaused = false;
        }

        private void OnCompleted(int level)
        {
            int nextLevel = level + 1;
            if(nextLevel >= m_Wheels.Length)
            {
                DebugLog.Error("Timer", "Timer Error");
                return;
            }

            m_Wheels[nextLevel].DoPushWheel(1);
        }

        private void OnSoltTrigger(TimerTask[] tasks)
        {
            if(tasks!=null && tasks.Length>0)
            {
                foreach(var task in tasks)
                {
                    if(m_Handlers.TryGetValue(task.Index,out TimerInstance handler))
                    {
                        if(handler.IsValid())
                        {
                            if(task.Trigger())
                            {
                                handler.Clear();
                                m_Handlers.Remove(task.Index);
                                m_TaskPool.Release(task);
                            }
                            else
                            {
                                AddTask(task,handler);
                            }
                        }else
                        {
                            m_Handlers.Remove(task.Index);
                            m_TaskPool.Release(task);
                        }
                    }else
                    {
                        m_TaskPool.Release(task);
                    }
                }
            }
        }


    }
}


