using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using KSTCEngine.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KSTCEngine.GPerf
{
    public class GPerfMonitor
    {
        public static GPerfMonitor Monitor = null;
        public static void Startup()
        {
            if(Monitor == null)
            {
                Monitor = new GPerfMonitor();

                GPerfPlatform.InitPlugin();

                GameObject gObj = new GameObject("GPerfMonitor");
                GameObject.DontDestroyOnLoad(gObj);
                Monitor.m_Behaviour = gObj.AddComponent<GPerfBehaviour>();

                //Monitor.OpenRecorder(RecorderType.File);
                Monitor.OpenRecorder(RecorderType.Console);
                Monitor.OpenRecorder(RecorderType.Remote);

                Monitor.OpenSampler(SamplerType.Battery);
                Monitor.OpenSampler(SamplerType.Device);
                Monitor.OpenSampler(SamplerType.CPU);
                Monitor.OpenSampler(SamplerType.FPS);
                Monitor.OpenSampler(SamplerType.Memory);
            }
        }

        public static void ShuntDown()
        {
            if(Monitor!=null)
            {
                GameObject.Destroy(Monitor.m_Behaviour.gameObject);
                Monitor.DoDispose();
                Monitor = null;
            }    
        }
        public float SampleInterval { get; set; } = 10.0f;

        private GPerfBehaviour m_Behaviour = null;

        private Dictionary<SamplerType, ISampler> m_SamplerDic = new Dictionary<SamplerType, ISampler>();
        private Dictionary<RecorderType, IRecorder> m_RecorderDic = new Dictionary<RecorderType, IRecorder>();

        private List<Record> recordList = new List<Record>();
        private SimpleObjectPool<Record> recordPool = null;
        private float m_ElapsedTime = 0.0f;

        private GPerfMonitor()
        {
            recordPool = new SimpleObjectPool<Record>(null, (record)=>{
                record.Type = SamplerType.None;
                record.Time = DateTime.Now;
                record.FrameIndex = Time.frameCount;

                record.Data = string.Empty;
            },3);
        }

        public void OpenSampler(SamplerType type)
        {
            if (m_SamplerDic.ContainsKey(type))
            {
                return;
            }
            ISampler sampler = null;
            switch (type)
            {
                case SamplerType.FPS:
                    sampler = new FPSSampler();
                    break;
                case SamplerType.Memory:
                    sampler = new MemorySampler();
                    break;
                case SamplerType.Device:
                    sampler = new DeviceSampler();
                    break;
                case SamplerType.Battery:
                    sampler = new BatterySampler();
                    break;
                case SamplerType.CPU:
                    sampler = new CPUSampler();
                    break;
                default:
                    sampler = null;
                    break;
            }
            if (sampler != null)
            {
                m_SamplerDic.Add(type, sampler);
            }
        }

        public void CloseSampler(SamplerType type)
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
            }

            if (recorder != null)
            {
                recorder.SetUserInfo();
                recorder.DoInit();
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

        public void DoUpdate(float deltaTime)
        {
            foreach (var kvp in m_SamplerDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }

            m_ElapsedTime += deltaTime;
            if(m_ElapsedTime>=SampleInterval)
            {
                bool isFailed = false;
                foreach (var kvp in m_SamplerDic)
                {
                    Record record = recordPool.Get();
                    if(kvp.Value.DoSample(record))
                    {
                        recordList.Add(record);
                    }else
                    {
                        isFailed = true;
                        break;
                    }
                }

                if(!isFailed)
                {
                    Record[] records = recordList.ToArray();
                    foreach (var kvp in m_RecorderDic)
                    {
                        kvp.Value.HandleRecords(records);
                    }
                }

                foreach(var record in recordList)
                {
                    recordPool.Release(record);
                }
                recordList.Clear();

                m_ElapsedTime -= SampleInterval;
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
