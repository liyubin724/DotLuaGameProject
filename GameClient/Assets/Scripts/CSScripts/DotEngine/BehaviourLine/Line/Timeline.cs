using DotEngine.BehaviourLine.Track;
using System;
using System.Collections.Generic;

namespace DotEngine.BehaviourLine.Line
{
    public enum TimelineState
    {
        None = 0,
        Playing,
        Paused,
        Finished,
    }

    public class Timeline
    {
        public TimelineState State { get; private set; }
        public Action<Timeline> FinishedCallback { get; set; }

        private List<Trackline> tracks = new List<Trackline>();

        private float realTimeLength = 0.0f;
        private LineContext context = null;
        private TimelineData data = null;

        public Timeline()
        { }

        public void SetData(LineContext context,TimelineData data,float timeScale)
        {
            this.context = context;
            this.data = data;
            realTimeLength = data.TimeLength * timeScale;

            for(int i =0;i<data.Tracks.Count;++i)
            {
                Trackline trackline = new Trackline();
                trackline.SetData(context, data.Tracks[i], timeScale);

                tracks.Add(trackline);
            }
        }

        public bool IsRunning()
        {
            return State == TimelineState.Playing || State == TimelineState.Paused;
        }

        public void Play()
        {
            if (State != TimelineState.Finished)
            {
                State = TimelineState.Playing;
            }
        }

        public void Pause()
        {
            if (State == TimelineState.Playing || State == TimelineState.None)
            {
                State = TimelineState.Paused;
                foreach (var track in tracks)
                {
                    track.DoPause();
                }
            }
        }

        public void Resume()
        {
            if (State == TimelineState.Paused)
            {
                State = TimelineState.Playing;
                foreach (var track in tracks)
                {
                    track.DoResume();
                }
            }
        }

        public void Stop()
        {
            if (State == TimelineState.Playing || State == TimelineState.Paused)
            {
                State = TimelineState.Finished;
                FinishedCallback?.Invoke(this);

                DoReset();
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if (State != TimelineState.Playing)
            {
                return;
            }

            foreach (var track in tracks)
            {
                track.DoUpdate(deltaTime);
            }

            realTimeLength -= deltaTime;
            if (realTimeLength <= 0)
            {
                Stop();
            }
        }

        private void DoReset()
        {
            State = TimelineState.None;
            foreach (var track in tracks)
            {
                track.DoDestroy();
            }
            tracks.Clear();
            realTimeLength = 0.0f;
        }

        public void DoDestroy()
        {
            DoReset();
            FinishedCallback = null;
            context = null;
        }
    }
}
