using DotEngine.Monitor.Recorder;
using DotEngine.Monitor.Sampler;
using System;
using System.Collections.Generic;

namespace DotEngine.Monitor
{
    public class MonitorSystem : IMonitorRecorder
    {
        private static MonitorSystem sm_MonitorSystem = null;

        public static MonitorSystem GetInstance()
        {
            if(sm_MonitorSystem == null)
            {
                sm_MonitorSystem = new MonitorSystem();
                sm_MonitorSystem.DoInit();
            }
            return sm_MonitorSystem;
        }

        private Dictionary<MonitorSamplerType, IMonitorSampler> m_SamplerDic = new Dictionary<MonitorSamplerType, IMonitorSampler>();
        private Dictionary<MonitorRecorderType, IMonitorRecorder> m_RecorderDic = new Dictionary<MonitorRecorderType, IMonitorRecorder>();

        private MonitorSystem()
        {
        }

        public void DoInit()
        {
        }

        public void HandleRecords(MonitorSamplerType type, MonitorRecord[] records)
        {
            foreach(var kvp in m_RecorderDic)
            {
                kvp.Value.HandleRecords(type, records);
            }
        }

        public void OpenSamplers(MonitorSamplerType[] types)
        {
            if(types!=null && types.Length>0)
            {
                Array.ForEach(types,(t) =>
                {
                    OpenSampler(t);
                });
            }
        }

        public void OpenSampler(MonitorSamplerType type)
        {
            if(m_SamplerDic.ContainsKey(type))
            {
                return;
            }
            IMonitorSampler sampler = null;
            switch(type)
            {
                case MonitorSamplerType.FPS: 
                    sampler = new FPSSampler(HandleRecords);
                    break;
                case MonitorSamplerType.Log:
                    sampler = new LogSampler(HandleRecords);
                    break;
                case MonitorSamplerType.ProcMemory:
                    sampler = new ProcMemorySampler(HandleRecords);
                    break;
                case MonitorSamplerType.ProfilerMemory:
                    sampler = new ProfilerMemorySampler(HandleRecords);
                    break;
                case MonitorSamplerType.XLuaMemory:
                    sampler = new XLuaMemorySampler(HandleRecords);
                    break;
                case MonitorSamplerType.USystemDevice:
                    sampler = new USystemDeviceSampler(HandleRecords);
                    break;
                default:
                    sampler = null;
                    break;
            }
            if(sampler!=null)
            {
                m_SamplerDic.Add(type, sampler);
            }
        }

        public void CloseSampler(MonitorSamplerType type)
        {
            if(m_SamplerDic.TryGetValue(type,out var sampler))
            {
                sampler.Dispose();
                m_SamplerDic.Remove(type);
            }
        }

        public void OpenRecorder(MonitorRecorderType type,params object[] values)
        {
            if(m_RecorderDic.ContainsKey(type))
            {
                return;
            }
            IMonitorRecorder recorder = null;
            switch(type)
            {
                case MonitorRecorderType.File:
                    {
                        if(values!=null && values.Length>0)
                        {
                            string rootDir = (string)values[0];
                            recorder = new FileRecorder(rootDir);
                        }
                    }
                    break;
                case MonitorRecorderType.Console:
                    recorder = new ConsoleRecorder();
                    break;
            }

            if(recorder!=null)
            {
                recorder.DoInit();
                m_RecorderDic.Add(type, recorder);
            }
        }

        public void CloseRecorder(MonitorRecorderType type)
        {
            if (m_RecorderDic.TryGetValue(type, out var recorder))
            {
                recorder.Dispose();
                m_RecorderDic.Remove(type);
            }
        }

        public void DoUpdate(float deltaTime)
        {
            foreach(var kvp in m_SamplerDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }

            foreach(var kvp in m_RecorderDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }

        public void Dispose()
        {
            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.Dispose();
            }
            m_SamplerDic.Clear();
            foreach (var kvp in m_RecorderDic)
            {
                kvp.Value.Dispose();
            }
            m_RecorderDic.Clear();

            sm_MonitorSystem = null;
        }
    }
}
