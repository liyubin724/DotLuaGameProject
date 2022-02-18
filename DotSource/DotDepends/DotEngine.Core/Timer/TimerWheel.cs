using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Timer
{
    internal delegate void OnWheelSlotTrigger(TimerTask[] tasks);
    internal delegate void OnWheelCompleted(int level);

    /// <summary>
    /// 时间轮定时器
    /// </summary>
    public class TimerWheel
    {
        private int currentSlotIndex = 0;
        private List<Dictionary<int, TimerTask>> m_AllTasks = new List<Dictionary<int, TimerTask>>();

        internal int Level { get; set; } = -1;
        internal int TickInMS { get; private set; } = 0;
        internal int SlotSize { get; private set; } = 0;

        internal int TotalTickInMS { get => TickInMS * SlotSize; }

        internal OnWheelSlotTrigger slotTriggerEvent = null;
        internal OnWheelCompleted completeEvent = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">时间轮序号</param>
        /// <param name="tickInMS">一刻度的时长，以毫秒计</param>
        /// <param name="slotSize">总的刻度数</param>
        public TimerWheel(int level, int tickInMS, int slotSize):this(tickInMS,slotSize)
        {
            Level = level;
        }

        public TimerWheel(int tickInMS, int slotSize)
        {
            TickInMS = tickInMS;
            SlotSize = slotSize;

            for (int i = 0; i < SlotSize; ++i)
            {
                m_AllTasks.Add(new Dictionary<int, TimerTask>());
            }
        }

        internal int AddTask(TimerTask task)
        {
            int slot = task.TriggerLeftInMS / TickInMS;
            task.TriggerLeftInMS = task.TriggerLeftInMS % TickInMS;

            int slotIndex = currentSlotIndex + slot;
            slotIndex %= SlotSize;

            m_AllTasks[slotIndex].Add(task.Index, task);

            return slotIndex;
        }

        internal TimerTask RemoveTask(TimerInstance handler)
        {
            if(m_AllTasks[handler.WheelSlotIndex].TryGetValue(handler.Index,out TimerTask task))
            {
                m_AllTasks[handler.WheelSlotIndex].Remove(handler.Index);
                return task;
            }
            return null;
        }

        internal void DoPushWheel(int pushNum)
        {
            for (int i = 0; i < pushNum; ++i)
            {
                ++currentSlotIndex;

                if(currentSlotIndex == SlotSize)
                {
                    currentSlotIndex = 0;
                    completeEvent?.Invoke(Level);
                }

                TimerTask[] tasks = m_AllTasks[currentSlotIndex].Values.ToArray();
                if(tasks != null && tasks.Length>0)
                {
                    m_AllTasks[currentSlotIndex].Clear();
                    slotTriggerEvent?.Invoke(tasks);
                }
            }
        }
    }
}