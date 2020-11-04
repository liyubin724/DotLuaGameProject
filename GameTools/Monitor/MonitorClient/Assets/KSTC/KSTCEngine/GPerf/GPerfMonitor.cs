using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
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

#if UNITY_EDITOR_WIN || UNITY_ANDROID
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeLoaded()
        {
            GPerfMonitor.GetInstance().DoInit();

            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Battery);
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.FPS);

            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.SystemMemory);
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.ProfilerMemory);
#if GPERF_XLUA
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.XLuaMemory);
#endif

            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Device);
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.App);
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.CPU);
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.FrameTime);
            GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Log);
        }
#endif

        private GameObject m_GObject = null;
        private GPerfBehaviour m_Behaviour = null;
        internal GPerfBehaviour Behaviour
        {
            get { return m_Behaviour; }
        }

        private bool m_IsRunning = false;
        private Dictionary<SamplerMetricType, ISampler> m_SamplerDic = new Dictionary<SamplerMetricType, ISampler>();
        private Dictionary<RecorderType, IRecorder> m_RecorderDic = new Dictionary<RecorderType, IRecorder>();

        private GPerfMonitor()
        {
        }

#if GPERF_XLUA
        public XLua.LuaEnv XLuaEnv { get; private set; }
        public void SetLuaEnv(XLua.LuaEnv luaEnv)
        {
            XLuaEnv = luaEnv;
        }
#endif

        public void DoInit()
        {
            m_GObject = new GameObject("GPerfMonitor");
            m_Behaviour = m_GObject.AddComponent<GPerfBehaviour>();
            UnityObject.DontDestroyOnLoad(m_GObject);
        }

        public void Startup()
        {
            if (!m_IsRunning)
            {
                foreach (var kvp in m_RecorderDic)
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
            if (m_IsRunning)
            {
                foreach (var kvp in m_SamplerDic)
                {
                    kvp.Value.DoEnd();
                }

                foreach (var kvp in m_RecorderDic)
                {
                    kvp.Value.DoEnd();
                }
            }

            m_IsRunning = false;
        }
        
        internal Record GetSamplerRecord(SamplerMetricType metricType)
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
            ISampler sampler = GPerfSamplerFactory.GetSampler(type);
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
            IRecorder recorder = GPerfRecorderFactory.GetRecorder(type);
            if (recorder != null)
            {
                recorder.DoStart();
                m_RecorderDic.Add(type, recorder);
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
                IRecorder recorder = kvp.Value;
                if(typeof(IHandleRecorder).IsAssignableFrom(recorder.GetType()))
                {
                    if(Debug.isDebugBuild)
                    {
                        Profiler.BeginSample($"GPerfSampler-HandleRecord:{recorder.Type}");
                        ((IHandleRecorder)recorder).HandleRecord(record);
                        Profiler.EndSample();
                    }else
                    {
                        ((IHandleRecorder)recorder).HandleRecord(record);
                    }
                }
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
                IRecorder recorder = kvp.Value;
                if (typeof(IIntervalRecorder).IsAssignableFrom(recorder.GetType()))
                {
                    ((IIntervalRecorder)recorder).DoUpdate(deltaTime);
                }
            }
        }

        public void DoDispose()
        {
            m_GObject = null;
            m_Behaviour = null;

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
