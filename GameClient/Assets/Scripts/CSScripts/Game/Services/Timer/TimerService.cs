using DotEngine.Framework.Services;
using DotEngine.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public class TimerService : Service, ITimerService
    {
        public readonly static string NAME = "TimerService";

        private HierarchicalTimerWheel hTimerWheel = null;
        public TimerService() : base(NAME)
        {
        }

        public override void DoRegistered()
        {
            hTimerWheel = new HierarchicalTimerWheel();
        }

        public override void DoUnregistered()
        {
            throw new System.NotImplementedException();
        }

        public TimerInstance AddEndTimer(float totalInSec, TimerEvent endEvent, object userData = null)
        {
            throw new System.NotImplementedException();
        }

        public TimerInstance AddIntervalTimer(float intervalInSec, TimerEvent intervalEvent, object userData = null)
        {
            throw new System.NotImplementedException();
        }

        public TimerInstance AddTickTimer(TimerEvent tickEvent, object userData = null)
        {
            throw new System.NotImplementedException();
        }

        public TimerInstance AddTimer(float intervalInSec, float totalInSec, TimerEvent intervalEvent, TimerEvent endEvent, object userData = null)
        {
            throw new System.NotImplementedException();
        }

        

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTimer(TimerInstance timer)
        {
            throw new System.NotImplementedException();
        }

        public void Resume()
        {
            throw new System.NotImplementedException();
        }
    }

}