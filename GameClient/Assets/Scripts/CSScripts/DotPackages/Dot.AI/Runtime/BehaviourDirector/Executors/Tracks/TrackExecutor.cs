using DotEngine.BD.Datas;
using System.Collections.Generic;

namespace DotEngine.BD.Executors
{
    public abstract class TrackExecutor : BDExecutor
    {
        private List<ActionExecutor> m_WaitingExecutors = new List<ActionExecutor>();
        private List<DurationActionExecutor> m_RunningExecutors = new List<DurationActionExecutor>();

        private float elapsedTime = 0.0f;

        public override void DoInit(CutsceneContext context,BDData data)
        {
            base.DoInit(context, data);

            TrackData trackData = (TrackData)data;
            
            for(int i =0;i< trackData.Actions.Count;++i)
            {
                ActionData actionData = trackData.Actions[i];
                ActionExecutor executor = BDExecutorFactory.GetInstance().RetainExecutor<ActionExecutor>(actionData.GetType());
                if(executor !=null)
                {
                    executor.DoInit(context, actionData);

                    m_WaitingExecutors.Add(executor);
                }
            }
        }

        public virtual void DoUpdate(float deltaTime)
        {
            if(m_WaitingExecutors.Count == 0 && m_RunningExecutors.Count == 0)
            {
                return;
            }

            elapsedTime += deltaTime;
            while(m_WaitingExecutors.Count>0)
            {
                ActionExecutor executor = m_WaitingExecutors[0];
                if(executor.FireTime <= elapsedTime)
                {
                    if(executor is EventActionExecutor eventActionExecutor)
                    {
                        eventActionExecutor.DoTrigger();

                        BDExecutorFactory.GetInstance().ReleaseExecutor(executor);
                    }else if(executor is DurationActionExecutor durationActionExecutor)
                    {
                        durationActionExecutor.DoEnter();

                        m_RunningExecutors.Add(durationActionExecutor);
                    }

                    m_WaitingExecutors.RemoveAt(0);
                }else
                {
                    break;
                }
            }

            if(m_RunningExecutors.Count>0)
            {
                for(int i =0;i<m_RunningExecutors.Count;)
                {
                    DurationActionExecutor executor = m_RunningExecutors[i];

                    executor.DoUpdate(deltaTime);

                    if(executor.EndTime <= elapsedTime)
                    {
                        executor.DoExit();

                        m_RunningExecutors.RemoveAt(i);
                        BDExecutorFactory.GetInstance().ReleaseExecutor(executor);
                    }else
                    {
                        ++i;
                    }
                }
            }

        }

        public void DoPause()
        {
            for(int i =0;i<m_RunningExecutors.Count;++i)
            {
                m_RunningExecutors[i].DoPause();
            }
        }

        public void DoResume()
        {
            for (int i = 0; i < m_RunningExecutors.Count; ++i)
            {
                m_RunningExecutors[i].DoResume();
            }
        }

        public override void DoReset()
        {
            for(int i =0;i<m_RunningExecutors.Count;++i)
            {
                m_RunningExecutors[i].DoExit();
                BDExecutorFactory.GetInstance().ReleaseExecutor(m_RunningExecutors[i]);
            }
            m_RunningExecutors.Clear();
            for(int i =0;i<m_WaitingExecutors.Count;++i)
            {
                BDExecutorFactory.GetInstance().ReleaseExecutor(m_WaitingExecutors[i]);
            }
            m_WaitingExecutors.Clear();

            elapsedTime = 0.0f;
            base.DoReset();
        }

    }
}
