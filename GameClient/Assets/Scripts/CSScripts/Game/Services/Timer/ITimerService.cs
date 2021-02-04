using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;
using DotEngine.Timer;
using SystemObject = System.Object;

namespace Game.Services
{
    public delegate void TimerEvent(TimerInstance timer,SystemObject userData);

    public interface ITimerService : IService, IUpdate
    {
        void Pause();
        void Resume();

        TimerInstance AddTickTimer(TimerEvent tickEvent, SystemObject userData = null);
        TimerInstance AddIntervalTimer(float intervalInSec, TimerEvent intervalEvent, SystemObject userData = null);
        TimerInstance AddEndTimer(float totalInSec, TimerEvent endEvent, SystemObject userData = null);
        TimerInstance AddTimer(float intervalInSec, float totalInSec, TimerEvent intervalEvent, TimerEvent endEvent, SystemObject userData = null);

        void RemoveTimer(TimerInstance timer);
    }
}