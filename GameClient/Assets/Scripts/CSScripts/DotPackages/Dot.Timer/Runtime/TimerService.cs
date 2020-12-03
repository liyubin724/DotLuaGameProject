﻿using DotEngine.Services;
using System;

namespace DotEngine.Timer
{
    public class TimerService : Servicer, IUpdate
    {
        public const string NAME = "TimerService";

        private HierarchicalTimerWheel hTimerWheel = null;
        public TimerService() :base(NAME)
        {
        }

        public override void DoRegister()
        {
            base.DoRegister();
            hTimerWheel = new HierarchicalTimerWheel();
        }

        public override void DoRemove()
        {
            hTimerWheel = null;
            base.DoRemove();
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            hTimerWheel.DoUpdate(deltaTime);
        }

        public void Pause()
        {
            hTimerWheel.Pause();
        }

        public void Resume()
        {
            hTimerWheel.Resume();
        }

        public TimerHandler AddTimer(float intervalInSec,
                                                float totalInSec,
                                                Action<object> intervalCallback,
                                                Action<object> endCallback,
                                                object userData)
        {
            return hTimerWheel.AddTimer(intervalInSec, totalInSec, intervalCallback, endCallback, userData);
        }

        public TimerHandler AddIntervalTimer(float intervalInSec,
                                                                    Action<object> intervalCallback, 
                                                                    object userData = null)
        {
            return hTimerWheel.AddIntervalTimer(intervalInSec, intervalCallback, userData);
        }

        public TimerHandler AddTickTimer(
            Action<object> intervalCallback,
            object userdata
            )
        {
            return hTimerWheel.AddTickTimer( intervalCallback, userdata);
        }

        public TimerHandler AddEndTimer(float totalInSec,
                                                                Action<object> endCallback,
                                                                object userData = null)
        {
            return hTimerWheel.AddEndTimer(totalInSec, endCallback, userData);
        }

        public bool RemoveTimer(TimerHandler handler)
        {
            return hTimerWheel.RemoveTimer(handler);
        }
    }
}
