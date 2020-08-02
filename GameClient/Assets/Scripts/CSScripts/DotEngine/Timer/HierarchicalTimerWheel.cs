using DotEngine.Generic;
using DotEngine.Log;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Timer
{
    /// <summary>
    /// 多层时间轮
    /// </summary>
    internal class HierarchicalTimerWheel
    {
        private UniqueIntID m_IndexCreator = new UniqueIntID();
        private ObjectPool<TimerTask> m_TaskPool = new ObjectPool<TimerTask>();

        private bool m_IsPaused = false;
        private List<TimerWheel> m_Wheels = new List<TimerWheel>();
        private TimerWheel m_Wheel = null;
        private Dictionary<int, TimerHandler> m_Handlers = new Dictionary<int, TimerHandler>();
        private float m_ElapseInMS = 0.0f;

       /// <summary>
       /// 初始化多层时间轮，目前默认生成5层
       /// </summary>
        internal HierarchicalTimerWheel()
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

        private TimerWheel CreateWheel(int level ,int tick,int size)
        {
            TimerWheel wheel = new TimerWheel(level, tick, size);
            wheel.completeEvent = OnCompleted;
            wheel.slotTriggerEvent = OnSoltTrigger;
            return wheel;
        }

        public TimerHandler AddTimer(
            float intervalInSec,
            float totalInSec,
            Action<object> intervalCallback,
            Action<object> endCallback,
            object userdata)
        {
            int index = m_IndexCreator.NextID;

            TimerTask task = m_TaskPool.Get();
            task.SetData(index, intervalInSec, totalInSec, intervalCallback, endCallback, userdata);

            return AddTask(task,null);
        }

        public TimerHandler AddIntervalTimer(
            float intervalInSec,
            Action<object> intervalCallback,
            object userdata)
        {
            return AddTimer(intervalInSec, 0, intervalCallback, null, userdata);
        }

        public TimerHandler AddEndTimer(
            float totalInSec,
            Action<object> endCallback,
            object userdata)
        {
            return AddTimer(0, totalInSec, null, endCallback, userdata);
        }

        private TimerHandler AddTask(TimerTask task,TimerHandler handler)
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
                handler = new TimerHandler()
                {
                    Index = task.Index,
                    WheelIndex = wheelIndex,
                    WheelSlotIndex = slotIndex,
                };
                m_Handlers.Add(task.Index, handler);
            }
            
            return handler;
        }

        public bool RemoveTimer(TimerHandler handler)
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
                LogUtil.LogError("Timer", "Timer Error");
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
                    if(m_Handlers.TryGetValue(task.Index,out TimerHandler handler))
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


