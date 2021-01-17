using DotEngine.BD.Datas;
using System.Collections.Generic;

namespace DotEngine.BD.Executors
{
    public abstract class GroupExecutor : BDExecutor
    {
        private List<TrackExecutor> m_Executors = new List<TrackExecutor>();

        public override void DoInit(CutsceneContext context,BDData data)
        {
            base.DoInit(context, data);

            GroupData groupData = (GroupData)data;
            for(int i =0;i<groupData.Tracks.Count;++i)
            {
                TrackData trackData = groupData.Tracks[i];
                TrackExecutor executor = BDExecutorFactory.GetInstance().RetainExecutor<TrackExecutor>(trackData.GetType());
                if(executor!=null)
                {
                    m_Executors.Add(executor);
                }
            }
        }

        public virtual void DoUpdate(float deltaTime)
        {
            for(int i =0;i<m_Executors.Count;++i)
            {
                m_Executors[i].DoUpdate(deltaTime);
            }
        }

        public void DoPause()
        {
            for (int i = 0; i < m_Executors.Count; ++i)
            {
                m_Executors[i].DoPause();
            }
        }

        public void DoResume()
        {
            for (int i = 0; i < m_Executors.Count; ++i)
            {
                m_Executors[i].DoResume();
            }
        }

        public override void DoReset()
        {
            for (int i = 0; i < m_Executors.Count; ++i)
            {
                m_Executors[i].DoReset();
            }
            m_Executors.Clear();

            base.DoReset();
        }
    }
}
