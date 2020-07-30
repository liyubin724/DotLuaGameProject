using DotEngine.Services;
using System;

namespace DotEngine.Timer
{
    public class TimerService : Service, IUpdate
    {
        public const string NAME = "TimerService";

        public float LuaUpdateInterval { get; set; } = 0.1f;

        private TimerManager timerMgr = null;
        public TimerService() :base(NAME)
        {
        }

        public override void DoRegister()
        {
            base.DoRegister();
            timerMgr = TimerManager.GetInstance();
        }

        public override void DoRemove()
        {
            base.DoRemove();

            timerMgr.DoDispose();
            timerMgr = null;
        }

        public void DoUpdate(float deltaTime)
        {
            timerMgr.DoUpdate(deltaTime);
        }

        public void Pause()
        {
            timerMgr.Pause();
        }

        public void Resume()
        {
            timerMgr.Resume();
        }

        public TimerTaskHandler AddTimer(float intervalInSec,
                                                float totalInSec,
                                                Action<object> intervalCallback,
                                                Action<object> endCallback,
                                                object userData)
        {
            return timerMgr.AddTimer(intervalInSec, totalInSec, intervalCallback, endCallback, userData);
        }

        public TimerTaskHandler AddIntervalTimer(float intervalInSec,
                                                                    Action<object> intervalCallback, 
                                                                    object userData = null)
        {
            return AddTimer(intervalInSec, 0f, intervalCallback, null, userData);
        }

        public TimerTaskHandler AddEndTimer(float totalInSec,
                                                                Action<object> endCallback,
                                                                object userData = null)
        {
            return AddTimer(totalInSec, totalInSec, null, endCallback, userData);
        }

        public bool RemoveTimer(TimerTaskHandler taskInfo)
        {
            return timerMgr.RemoveTimer(taskInfo);
        }
    }
}
