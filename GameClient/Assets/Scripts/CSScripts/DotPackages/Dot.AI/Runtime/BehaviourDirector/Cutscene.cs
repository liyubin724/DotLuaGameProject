using DotEngine.AI.BD.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BD
{
    public delegate void CutsceneHandler(Cutscene cutscene);

    public enum CutsceneState
    {
        Inactive,
        Playing,
        Paused
    }

    public class Cutscene
    {
        public float Duration = 0.0f;
        public List<TrackGroup> Groups = new List<TrackGroup>();

        public event CutsceneHandler OnCutsceneStarted;
        public event CutsceneHandler OnCutsceneFinished;
        public event CutsceneHandler OnCutscenePaused;

        public CutsceneContext Context { get; set; } = null;
        public CutsceneState State { get; private set; } = CutsceneState.Inactive;

        public void Play()
        {

        }

        public void PlayAt(float startTime)
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Stop()
        {

        }

        public void DoUpdate(float deltaTime)
        {

        }
        
    }
}
