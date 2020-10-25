using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using SystemObject = System.Object;
using System;

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

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeLoaded()
        {
            GPerfMonitor.GetInstance().DoInit();
        }

        private GameObject m_GObject = null;
        private GPerfBehaviour m_Behaviour = null;
        internal GPerfBehaviour Behaviour
        {
            get { return m_Behaviour; }
        }

        private Dictionary<string, Action<GPerfMonitorAction>> m_ActionDic = new Dictionary<string, Action<GPerfMonitorAction>>();

        private object m_MonitorLock = new object();
        private bool m_IsRunning = false;
        private List<GPerfMonitorAction> m_Actions = new List<GPerfMonitorAction>();
        private Dictionary<SamplerMetricType, ISampler> m_SamplerDic = new Dictionary<SamplerMetricType, ISampler>();
        private Dictionary<RecorderType, IRecorder> m_RecorderDic = new Dictionary<RecorderType, IRecorder>();

        private Thread m_MonitorThread = null;

        public int FrameRate { get; set; } = 10;
        private float m_FrameIntervalInS = 1.0f;
        private int m_FrameIntervalInMS = 1000;

        private GPerfMonitor()
        {
            m_ActionDic.Add(GPerfMonitorAction.START_ACTION, OnStartAction);
            m_ActionDic.Add(GPerfMonitorAction.END_ACTION, OnEndAction);
            m_ActionDic.Add(GPerfMonitorAction.OPEN_SAMPLER_ACTION,OnOpenSamplerAction);
            m_ActionDic.Add(GPerfMonitorAction.CLOSE_SAMPLER_ACTION, OnCloseSamplerAction);
            m_ActionDic.Add(GPerfMonitorAction.OPEN_RECORDER_ACTION, OnOpenRecorderAction);
            m_ActionDic.Add(GPerfMonitorAction.CLOSE_RECORDER_ACTION, OnCloseRecorderAction);
        }

        public void DoInit()
        {
            m_GObject = new GameObject("GPerfMonitor");
            m_Behaviour = m_GObject.AddComponent<GPerfBehaviour>();
            UnityObject.DontDestroyOnLoad(m_GObject);

            m_FrameIntervalInS = 1.0f / FrameRate;

            m_FrameIntervalInMS = (int)(m_FrameIntervalInS * 1000);
            m_MonitorThread = new Thread(OnThreadUpdate);
            m_MonitorThread.Start();
        }

        public void Startup()
        {
            lock (m_MonitorLock)
            {
                m_Actions.Add(new GPerfMonitorAction()
                {
                    ActionName = GPerfMonitorAction.START_ACTION,
                });
            }
        }

        public void Shuntdown()
        {
            lock (m_MonitorLock)
            {
                m_Actions.Add(new GPerfMonitorAction()
                {
                    ActionName = GPerfMonitorAction.END_ACTION,
                });
            }
        }
        
        private void OnThreadUpdate()
        {
            while(true)
            {
                lock(m_MonitorLock)
                {
                    while(m_Actions.Count>0)
                    {
                        GPerfMonitorAction mAction = m_Actions[0];
                        m_Actions.RemoveAt(0);

                        if (m_ActionDic.TryGetValue(mAction.ActionName, out var action))
                        {
                            action.Invoke(mAction);
                        }
                    }
                }

                Thread.Sleep(m_FrameIntervalInMS);

                if(m_IsRunning)
                {
                    foreach (var kvp in m_SamplerDic)
                    {
                        kvp.Value.DoUpdate(m_FrameIntervalInS);
                    }

                    foreach (var kvp in m_RecorderDic)
                    {
                        //kvp.Value.DoUpdate(deltaTime);
                    }
                }
            }
        }

        private void OnStartAction(GPerfMonitorAction action)
        {
            if(m_IsRunning)
            {
                return;
            }

            foreach (var kvp in m_RecorderDic)
            {
                kvp.Value.DoStart();
            }
            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.DoStart();
            }
            m_IsRunning = true;
        }

        private void OnEndAction(GPerfMonitorAction action)
        {
            if(!m_IsRunning)
            {
                return;
            }

            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.DoEnd();
            }

            foreach (var kvp in m_RecorderDic)
            {
                kvp.Value.DoEnd();
            }
            m_IsRunning = false;
        }

        private void OnOpenSamplerAction(GPerfMonitorAction action)
        {

        }
        private void OnCloseSamplerAction(GPerfMonitorAction action)
        {

        }
        private void OnOpenRecorderAction(GPerfMonitorAction action)
        {

        }
        private void OnCloseRecorderAction(GPerfMonitorAction action)
        {

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

                foreach (var kvp in m_SamplerDic)
                {
                    if(kvp.Value.FreqType == SamplerFreqType.Start)
                    {
                        //recorder.HandleRecord(kvp.Value.GetRecord());
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
                //kvp.Value.HandleRecord(record);
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
                //kvp.Value.DoUpdate(deltaTime);
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
