using DotEngine.Framework.Services;
using DotEngine.Timer;

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
            hTimerWheel = null;
        }

        public TimerInstance AddEndTimer(float totalInSec, TimerEventHandler endEvent, object userdata = null)
        {
            return hTimerWheel.AddEndTimer(totalInSec, endEvent, userdata);
        }

        public TimerInstance AddIntervalTimer(float intervalInSec, TimerEventHandler intervalEvent, object userdata = null)
        {
            return hTimerWheel.AddIntervalTimer(intervalInSec, intervalEvent, userdata);
        }

        public TimerInstance AddTickTimer(TimerEventHandler tickEvent, object userdata = null)
        {
            return hTimerWheel.AddTickTimer(tickEvent, userdata);
        }

        public TimerInstance AddTimer(float intervalInSec, float totalInSec, TimerEventHandler intervalEvent, TimerEventHandler endEvent, object userData = null)
        {
            return hTimerWheel.AddTimer(intervalInSec, totalInSec, intervalEvent, endEvent, userData);
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            hTimerWheel.DoUpdate(deltaTime);
        }

        public void Pause()
        {
            hTimerWheel.Pause();
        }

        public void RemoveTimer(TimerInstance timer)
        {
            hTimerWheel.RemoveTimer(timer);
        }

        public void Resume()
        {
            hTimerWheel.Resume();
        }
    }

}