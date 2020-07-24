namespace DotEngine.Timer
{
    public class TimerTaskHandler
    {
        internal long taskID = -1;
        internal int wheelIndex = -1;
        internal int wheelSlotIndex = -1;

        internal TimerTaskHandler()
        {
        }

        internal bool IsValid()
        {
            return taskID > 0 && wheelIndex > 0 && wheelSlotIndex > 0;
        }

        internal void Clear()
        {
            taskID = -1;
            wheelIndex = -1;
            wheelSlotIndex = -1;
        }

        public override string ToString()
        {
            return $"TimerTaskInfo:{{taskID = {taskID},wheelIndex = {wheelIndex},wheelSlotIndex = {wheelSlotIndex}}}";
        }
    }
}
