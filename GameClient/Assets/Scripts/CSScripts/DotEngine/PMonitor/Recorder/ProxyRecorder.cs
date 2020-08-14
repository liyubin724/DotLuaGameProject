using DotEngine.PMonitor.Sampler;
using System.Collections.Generic;

namespace DotEngine.PMonitor.Recorder
{
    public class ProxyRecorder : IRecorder
    {
        public RecorderCategory Category { get; } = RecorderCategory.None;

        private Dictionary<RecorderCategory, IRecorder> m_RecorderDic = new Dictionary<RecorderCategory, IRecorder>();

        public void Init()
        {

        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var kvp in m_RecorderDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }

        public void OpenRecorder(RecorderCategory category,params object[] values)
        {
            if (!m_RecorderDic.TryGetValue(category, out var recorder))
            {
                if(category == RecorderCategory.File)
                {
                    if(values.Length>0)
                    {
                        string rootDir = (string)values[0];
                        if(!string.IsNullOrEmpty(rootDir))
                        {
                            recorder = new FileRecorder(rootDir);
                        }
                    }
                }else if(category == RecorderCategory.Profiler)
                {
                    recorder = new ProfilerRecorder();
                }

                if(recorder!=null)
                {
                    recorder.Init();
                    m_RecorderDic.Add(category, recorder);
                }
            }
        }

        public void CloseRecorder(RecorderCategory category)
        {
            if(m_RecorderDic.TryGetValue(category,out var recorder))
            {
                recorder.Dispose();
                m_RecorderDic.Remove(category);
            }
        }

        public void HandleRecord(SamplerCategory category, Record[] records)
        {
            if(m_RecorderDic.Count>0)
            {
                foreach(var kvp in m_RecorderDic)
                {
                    kvp.Value.HandleRecord(category, records);
                }    
            }
        }

        public void Dispose()
        {
            foreach (var kvp in m_RecorderDic)
            {
                kvp.Value.Dispose();
            }
            m_RecorderDic.Clear();
        }
    }
}
