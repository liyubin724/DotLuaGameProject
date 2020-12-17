using DotEngine.BD.Datas;
using System.Collections.Generic;

namespace DotEngine.BD.Executors
{
    public enum CutsceneState
    {
        Invalid = 0,
        Playing,
        Paused,
    }

    public delegate void CutsceneHandler(CutsceneExecutor cutscene);

    public class CutsceneExecutor
    {
        public CutsceneState State { get; private set; } = CutsceneState.Invalid;

        public event CutsceneHandler OnCutsceneStarted;
        public event CutsceneHandler OnCutsceneStopped;
        public event CutsceneHandler OnCutscenePaused;
        public event CutsceneHandler OnCutsceneResumed;

        public event CutsceneHandler OnCutsceneFinished;

        private CutsceneContext m_Context = null;
        private CutsceneData m_Data = null;

        private List<GroupExecutor> m_Executors = new List<GroupExecutor>();

        public void SetData(CutsceneContext context,CutsceneData cutsceneData)
        {
            m_Context = context;
            m_Data = cutsceneData;
            
            for(int i =0;i<cutsceneData.Groups.Count;++i)
            {
                GroupData groupData = cutsceneData.Groups[i];
                GroupExecutor executor = BDExecutorFactory.GetInstance().RetainExecutor<GroupExecutor>(groupData.GetType());
                if(executor!=null)
                {
                    m_Executors.Add(executor);
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if(State != CutsceneState.Playing)
            {
                return;
            }

            for(int i =0;i<m_Executors.Count;++i)
            {
                m_Executors[i].DoUpdate(deltaTime);
            }
        }

        public void Play()
        {
            if(State == CutsceneState.Invalid)
            {
                State = CutsceneState.Playing;
            }
        }

        public void Stop()
        {
            if(State!= CutsceneState.Invalid)
            {
                for (int i = 0; i < m_Executors.Count; ++i)
                {
                    m_Executors[i].DoReset();

                    BDExecutorFactory.GetInstance().ReleaseExecutor(m_Executors[i]);
                }
                m_Executors.Clear();

                State = CutsceneState.Invalid;
            }
        }

        public void Pause()
        {
            if(State == CutsceneState.Playing)
            {
                for (int i = 0; i < m_Executors.Count; ++i)
                {
                    m_Executors[i].DoPause();
                }

                State = CutsceneState.Paused;
            }
        }

        public void Resume()
        {
            if (State == CutsceneState.Paused)
            {
                for (int i = 0; i < m_Executors.Count; ++i)
                {
                    m_Executors[i].DoResume();
                }

                State = CutsceneState.Playing;
            }
        }
    }
}
