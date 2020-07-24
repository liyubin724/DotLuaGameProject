using DotEngine.Generic;

namespace DotEngine.Timer
{
    internal delegate void OnWheelSlotTrigger(int index, TimerTask[] tasks);
    internal delegate void OnWheelTurnOnce(int index);

    /// <summary>
    /// 时间轮定时器
    /// </summary>
    internal sealed class TimerWheel
    {
        //用于多层时间轮层次索引
        private int index = 0;
        private int tickInMS = 0;
        private int slotSize = 0;

        internal int TickInMS { get => tickInMS; }
        internal int TotalTickInMS{ get => tickInMS * slotSize; }

        private int currentSlotIndex = 0;
        private ListDictionary<long, TimerTask>[] slotTasks = null;

        internal OnWheelSlotTrigger slotTriggerEvent = null;
        internal OnWheelTurnOnce turnOnceEvent = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">时间轮序号</param>
        /// <param name="tickInMS">一刻度的时长，以毫秒计</param>
        /// <param name="slotSize">总的刻度数</param>
        internal TimerWheel(int index, int tickInMS, int slotSize)
        {
            this.index = index;
            this.tickInMS = tickInMS;
            this.slotSize = slotSize;

            slotTasks = new ListDictionary<long, TimerTask>[slotSize];
        }

        internal int AddTimerTask(TimerTask task)
        {
            if(task.RemainingInMS>=TotalTickInMS)
            {
                return -1;
            }

            int slotIndex = task.RemainingInMS / tickInMS;
            if(slotIndex == 0)
            {
                task.RemainingInMS = 0;
            }else
            {
                task.RemainingInMS = task.RemainingInMS % tickInMS;
            }
            ++slotIndex;
            slotIndex = slotIndex % slotSize;

            ListDictionary<long, TimerTask> taskDic = slotTasks[slotIndex];
            if(taskDic == null)
            {
                taskDic = new ListDictionary<long, TimerTask>();
                slotTasks[slotIndex] = taskDic;
            }
            taskDic.Add(task.ID, task);

            return slotIndex;
        }

        internal bool RemoveTimerTask(int slotIndex,long taskID)
        {
            ListDictionary<long, TimerTask> taskDic = slotTasks[slotIndex];
            if(taskDic == null)
            {
                return false;
            }
            if(taskDic.ContainsKey(taskID))
            {
                taskDic.Remove(taskID);
                return true;
            }
            return false;
        }

        internal void DoTimerTurn(int turnNum)
        {
            for(int i =0;i<turnNum;++i)
            {
                ++currentSlotIndex;

                ListDictionary<long, TimerTask> taskDic = slotTasks[currentSlotIndex];
                if(taskDic!=null && taskDic.Count>0)
                {
                    slotTriggerEvent?.Invoke(index, taskDic.Values);
                    taskDic.Clear();
                }

                if (currentSlotIndex == slotSize)
                {
                    currentSlotIndex = 0;
                    turnOnceEvent?.Invoke(index);
                }
            }
        }
    }
}