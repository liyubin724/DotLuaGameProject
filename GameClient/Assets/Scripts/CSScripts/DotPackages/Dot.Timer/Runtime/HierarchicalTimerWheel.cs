using DotEngine.Generic;
using DotEngine.Pool;
using System;
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
        private UniqueIntID m_IndexCreator = new UniqueIntID();
        private ItemObjectPool<TimerTask> m_TaskPool = new ItemObjectPool<TimerTask>();

        private bool m_IsPaused = false;
        private List<TimerWheel> m_Wheels = new List<TimerWheel>();
        private TimerWheel m_Wheel = null;
        private Dictionary<int, TimerInstance> m_Handlers = new Dictionary<int, TimerInstance>();
        private float m_ElapseInMS = 0.0f;

        /// <summary>
        /// 初始化多层时间轮，目前默认生成5层
        /// </summary>
        public HierarchicalTimerWheel()
        {
            TimerWheel wheel0 = CreateWheel(0, 100, 10);
            TimerWheel wheel1 = CreateWheel(1, 100 * 10, 60);
            TimerWheel wheel2 = CreateWheel(2, 100 * 10 * 60, 60);
            TimerWheel wheel3 = CreateWheel(3, 100 * 10 * 60 * 60, 24);
            TimerWheel wheel4 = CreateWheel(4, 100 * 10 * 60 * 60 * 24, 30);

            m_Wheel = wheel0;
            m_Wheels.Add(wheel0);
            m_Wheels.Add(wheel1);
            m_Wheels.Add(wheel2);
            m_Wheels.Add(wheel3);
            m_Wheels.Add(wheel4);
        }

        private TimerWheel CreateWheel(int level, int tick, int size)
        {
            TimerWheel wheel = new TimerWheel(level, tick, size);
            wheel.completeEvent = OnCompleted;
            wheel.slotTriggerEvent = OnSoltTrigger;
            return wheel;
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
            return AddTimer(m_Wheel.TickInMS * 0.001f, 0, intervalCallback, null, userdata);
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
            for (int i = 0; i < m_Wheels.Count; ++i)
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

                if (m_ElapseInMS >= m_Wheel.TickInMS)
                {
                    int count = Mathf.FloorToInt(m_ElapseInMS / m_Wheel.TickInMS);
                    m_Wheel.DoPushWheel(count);

                    m_ElapseInMS %= m_Wheel.TickInMS;
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
            if(nextLevel >= m_Wheels.Count)
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


