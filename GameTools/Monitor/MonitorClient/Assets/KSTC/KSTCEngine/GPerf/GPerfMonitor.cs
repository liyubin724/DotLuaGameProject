using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityObject = UnityEngine.Object;

namespace KSTCEngine.GPerf
{
    public class GPerfMonitor
    {
        private static GPerfMonitor sm_Instance = null;
        public static GPerfMonitor GetInstance()
        {
            if(sm_Instance == null)
            {
                sm_Instance = new GPerfMonitor();

                GPerfPlatform.InitPlugin();
            }
            return sm_Instance;
        }

        private GameObject m_GObject = null;

        private Dictionary<SamplerMetricType, ISampler> m_SamplerDic = new Dictionary<SamplerMetricType, ISampler>();
        private Dictionary<RecorderType, IRecorder> m_RecorderDic = new Dictionary<RecorderType, IRecorder>();
       
        private bool m_IsRunning = false;

        private GPerfMonitor()
        {
        }

        public void Startup()
        {
            if(m_GObject == null)
            {
                m_GObject = new GameObject("GPerfMonitor");
                m_GObject.AddComponent<GPerfBehaviour>();

                UnityObject.DontDestroyOnLoad(m_GObject);
            }

            if(!m_IsRunning)
            {
                foreach(var kvp in m_RecorderDic)
                {
                    kvp.Value.DoStart();
                }

                foreach (var kvp in m_SamplerDic)
                {
                    kvp.Value.DoStart();
                }
            }
            m_IsRunning = true;
        }

        public void Shuntdown()
        {
            if(m_IsRunning)
            {
                foreach (var kvp in m_SamplerDic)
                {
                    kvp.Value.DoEnd();
                }

                foreach(var kvp in m_RecorderDic)
                {
                    kvp.Value.DoEnd();
                }
            }

            m_IsRunning = false;
        }

        public Record GetSamplerRecord(SamplerMetricType metricType)
        {
            if(m_SamplerDic.TryGetValue(metricType,out ISampler sampler))
            {
                return sampler.GetRecord();
            }
            return null;
        }

        public void OpenSampler(SamplerMetricType type)
        {
            if (m_SamplerDic.ContainsKey(type))
            {
                return;
            }
            ISampler sampler = null;
            switch (type)
            {
                case SamplerMetricType.FPS:
                    sampler = new FPSSampler();
                    break;
                case SamplerMetricType.Memory:
                    sampler = new MemorySampler();
                    break;
                case SamplerMetricType.ProfilerMemory:
                    sampler = new ProfilerMemorySampler();
                    break;
                case SamplerMetricType.Device:
                    sampler = new DeviceSampler();
                    break;
                case SamplerMetricType.App:
                    sampler = new AppSampler();
                    break;
                case SamplerMetricType.Battery:
                    sampler = new BatterySampler();
                    break;
                case SamplerMetricType.CPU:
                    sampler = new CPUSampler();
                    break;
                case SamplerMetricType.FrameTime:
                    sampler = new FrameTimeSampler();
                    break;
                case SamplerMetricType.Log:
                    sampler = new LogSampler();
                    break;
                default:
                    sampler = null;
                    break;
            }

            if (sampler != null)
            {
                sampler.OnSampleRecord = OnHandleRecord;

                sampler.DoStart();
                m_SamplerDic.Add(type, sampler);
            }
        }

        public void CloseSampler(SamplerMetricType type)
        {
            if (m_SamplerDic.TryGetValue(type, out var sampler))
            {
                sampler.DoDispose();
                m_SamplerDic.Remove(type);
            }
        }

        public void OpenRecorder(RecorderType type)
        {
            if (m_RecorderDic.ContainsKey(type))
            {
                return;
            }
            IRecorder recorder = null;
            switch (type)
            {
                case RecorderType.File:
                    recorder = new FileRecorder();
                    break;
                case RecorderType.Console:
                    recorder = new ConsoleRecorder();
                    break;
                case RecorderType.Remote:
                    recorder = new RemoteRecorder();
                    break;
            }

            if (recorder != null)
            {
                recorder.SetUserInfo();

                recorder.DoStart();
                m_RecorderDic.Add(type, recorder);

                foreach (var kvp in m_SamplerDic)
                {
                    if(kvp.Value.FreqType == SamplerFreqType.Start)
                    {
                        recorder.HandleRecord(kvp.Value.GetRecord());
                    }
                }
            }
        }

        public void CloseRecorder(RecorderType type)
        {
            if (m_RecorderDic.TryGetValue(type, out var recorder))
            {
                recorder.DoDispose();
                m_RecorderDic.Remove(type);
            }
        }

        private void OnHandleRecord(Record record)
        {
            foreach(var kvp in m_RecorderDic)
            {
                kvp.Value.HandleRecord(record);
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if(!m_IsRunning)
            {
                return;
            }

            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }

            foreach(var kvp in m_RecorderDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }

        public void DoDispose()
        {
            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.DoDispose();
            }
            m_SamplerDic.Clear();

            foreach (var kvp in m_RecorderDic)
            {
                kvp.Value.DoDispose();
            }
            m_RecorderDic.Clear();
        }
    }
}
