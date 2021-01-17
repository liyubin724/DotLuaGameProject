using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BD
{
    public delegate void CutsceneHandler(Cutscene cutscene);

    public enum CutsceneState
    {
        Inactive = 0,
        Playing,
        Paused,
    }

    public class Cutscene
    {
        public float Duration = 5.0f;
        public bool IsLooping = false;

        public bool IsSkippable = false;

        public event CutsceneHandler CutsceneStarted;
        public event CutsceneHandler CutsceneFinished;
        public event CutsceneHandler CutscenePaused;
        public event CutsceneHandler CutsceneResumed;

        public void Play()
        {

        }

        public void PlayAtTime()
        {

        }

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Skip()
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
