using DotEngine.Framework.Updater;
using SystemObject = System.Object;

namespace DotEngine.Framework.Services
{
    public delegate void TimerEvent(TimerInstance timer,SystemObject userData);

    public class TimerInstance
    { }

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