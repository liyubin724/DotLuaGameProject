using DotEngine.PMonitor.Recorder;
using DotEngine.PMonitor.Sampler;
using DotEngine.Services;
using System.Collections.Generic;

namespace DotEngine.PMonitor
{
    public class MonitorService : Service,IUpdate
    {
        public const string NAME = "MonitorService";

        private Dictionary<SamplerCategory, ISampler> m_SamplerDic = null;
        private ProxyRecorder m_Recorder = null;

        public MonitorService() : base(NAME)
        {
        }

        public void OpenSampler(SamplerCategory category)
        {
            if(!m_SamplerDic.TryGetValue(category,out var sampler))
            {
                if(category == SamplerCategory.FPS)
                {
                    sampler = new FPSSampler(m_Recorder);
                }else if(category == SamplerCategory.Memory)
                {
                    sampler = new MemorySampler(m_Recorder);
                }else if(category == SamplerCategory.Log)
                {
                    sampler = new LogSampler(m_Recorder);
                }else if(category == SamplerCategory.System)
                {
                    sampler = new SystemSampler(m_Recorder);
                }

                if(sampler != null)
                {
                    m_SamplerDic.Add(category, sampler);
                }
            }
        }

        public void CloseSampler(SamplerCategory category)
        {
            if (m_SamplerDic.TryGetValue(category, out var sampler))
            {
                m_SamplerDic.Remove(category);
                sampler.Dispose();
            }
        }

        public void OpenFileRecorder(string logDir)
        {
            m_Recorder.OpenRecorder(RecorderCategory.File,logDir);
        }

        public void CloseFileRecorder()
        {
            m_Recorder.CloseRecorder(RecorderCategory.File);
        }

        public override void DoRegister()
        {
            m_Recorder = new ProxyRecorder();
            m_SamplerDic = new Dictionary<SamplerCategory, ISampler>();
        }

        public override void DoRemove()
        {
            m_Recorder.Dispose();
            m_Recorder = null;

            foreach(var kvp in m_SamplerDic)
            {
                kvp.Value.Dispose();
            }
            m_SamplerDic = null;
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }
    }
}
