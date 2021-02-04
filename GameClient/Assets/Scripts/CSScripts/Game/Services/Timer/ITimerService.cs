using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;
using DotEngine.Timer;
using SystemObject = System.Object;

namespace Game.Services
{
    public interface ITimerService : IService, IUpdate
    {
        void Pause();
        void Resume();

        TimerInstance AddTickTimer(TimerEventHandler tickEvent, SystemObject userdata = null);
        TimerInstance AddIntervalTimer(float intervalInSec, TimerEventHandler intervalEvent, SystemObject userdata = null);
        TimerInstance AddEndTimer(float totalInSec, TimerEventHandler endEvent, SystemObject userdata = null);
        TimerInstance AddTimer(float intervalInSec, float totalInSec, TimerEventHandler intervalEvent, TimerEventHandler endEvent, SystemObject userdata = null);

        void RemoveTimer(TimerInstance timer);
    }
}