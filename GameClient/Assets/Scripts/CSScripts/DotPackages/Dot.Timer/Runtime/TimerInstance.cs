﻿namespace DotEngine.Timer
{
    public class TimerInstance
    {
        internal int Index { get; set; } = -1;
        internal int WheelIndex { get; set; } = -1;
        internal int WheelSlotIndex { get; set; } = -1;

        public TimerInstance()
        {
        }

        internal bool IsValid()
        {
            return Index >= 0 && WheelIndex >= 0 && WheelSlotIndex >= 0;
        }

        internal void Clear()
        {
            Index = -1;
            WheelIndex = -1;
            WheelSlotIndex = -1;
        }

        public override string ToString()
        {
            return $"TimerInstance:{{Index = {Index},wheelIndex = {WheelIndex},wheelSlotIndex = {WheelSlotIndex}}}";
        }
    }
}
