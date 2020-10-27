using Google.Protobuf;
using Gperf.U3D;
using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Recorder
{
    public class RemoteRecorder : GPerfIntervalRecorder
    {
        private DateTime m_OrgTime;
        private GPerfSession m_Session = null;

        private string m_URL = "http://hb.ix2.cn:16408/u3d/uploadStats";
        public RemoteRecorder():base(RecorderType.Remote)
        {
            m_OrgTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        }

        public override void DoStart()
        {
            m_Session = new GPerfSession();

            AppRecord appRecord = (AppRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.App);
            m_Session.App = new GPerfApp();
            m_Session.App.Identifier = appRecord.Identifier;
            m_Session.App.InstallName = appRecord.InstallName;
            m_Session.App.ProductName = appRecord.ProductName;
            m_Session.App.Version = appRecord.Version;
            m_Session.App.UnityVersion = appRecord.EngineVersion;

            DeviceRecord deviceRecord = (DeviceRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Device);
            m_Session.Device = new GPerfDevice();
            m_Session.Device.Name = deviceRecord.Name;
            m_Session.Device.Model = deviceRecord.Model;
            m_Session.Device.UniqueIdentifier = deviceRecord.UniqueIdentifier;

            m_Session.Device.GraphicsName = deviceRecord.GraphicsName;
            m_Session.Device.GraphicsType = deviceRecord.GraphicsType;
            m_Session.Device.GraphicsVendor = deviceRecord.GraphicsVendor;
            m_Session.Device.GraphicsVersion = deviceRecord.GraphicsVersion;
            m_Session.Device.GraphicsMemoryInMb = deviceRecord.GraphicsMemoryInMB;

            m_Session.Device.SystemMemoryInMb = deviceRecord.SystemMemorySize;

        }

        public override void DoRecord()
        {
            GPerfSample sample = new GPerfSample();
            sample.Timestamp = (uint)((DateTime.Now - m_OrgTime).Ticks / 10000000);

            FPSRecord fpsRecord = (FPSRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.FPS);
            sample.Fps = new GPerfFPS();
            sample.Fps.Fps = fpsRecord.FPS;
            sample.Fps.DeltaTimeInMS = fpsRecord.DeltaTimeInMS;

            CPURecord cpuRecord = (CPURecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.CPU);
            sample.Cpu = new GPerfCPU();
            sample.Cpu.CoreCount = cpuRecord.CoreCount;
            sample.Cpu.Frequency = cpuRecord.Frequency;
            sample.Cpu.UsageRate = cpuRecord.UsageRate;
            sample.Cpu.CoreFrequency.AddRange(cpuRecord.CoreFrequency);

            SystemMemoryRecord memoryRecord = (SystemMemoryRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.SystemMemory);
            sample.SystemMemory = new GPerfSystemMemory();
            sample.SystemMemory.TotalInKb = memoryRecord.TotalMemInKB;
            sample.SystemMemory.AvailableInKb = memoryRecord.AvailableMemInKB;
            sample.SystemMemory.ThresholdInKb = memoryRecord.ThresholdInKB;
            sample.SystemMemory.IsLow = memoryRecord.IsLowMem;
            sample.SystemMemory.PssInKb = memoryRecord.PSSMemInKB;

            ProfilerMemoryRecord profilerMemoryRecord = (ProfilerMemoryRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.ProfilerMemory);
            sample.ProfilerMemory = new GPerfProfilerMemory();
            sample.ProfilerMemory.MonoHeapSizeInKb = profilerMemoryRecord.MonoHeapSizeInKB;
            sample.ProfilerMemory.MonoUsedSizeInKb = profilerMemoryRecord.MonoUsedSizeInKB;
            sample.ProfilerMemory.TempAllocatorSizeInKb = profilerMemoryRecord.TempAllocatorSizeInKB;
            sample.ProfilerMemory.TotalAllocatorSizeInKb = profilerMemoryRecord.TotalAllocatorSizeInKB;
            sample.ProfilerMemory.TotalReservedSizeInKb = profilerMemoryRecord.TotalReservedSizeInKB;
            sample.ProfilerMemory.TotalUnusedReservedSizeInKb = profilerMemoryRecord.TotalUnusedReservedSizeInKB;
            sample.ProfilerMemory.AllocatedForGraphicsDriverInKb = profilerMemoryRecord.AllocatedForGraphicsDriverInKB;

            BatteryRecord batteryRecord = (BatteryRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Battery);
            sample.Battery = new GPerfBattery();
            sample.Battery.Temperature = batteryRecord.Temperature;
            sample.Battery.Status = batteryRecord.Status;
            sample.Battery.Rate = batteryRecord.Rate;

            FrameTimeRecord frameTimeRecord = (FrameTimeRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.FrameTime);
            sample.FrameTime = new GPerfFrameTime();
            sample.FrameTime.PlayerLoopTime = frameTimeRecord.PlayerLoopTime;
            sample.FrameTime.RenderingTime = frameTimeRecord.RenderingTime;
            sample.FrameTime.ScriptTime = frameTimeRecord.ScriptTime;
            sample.FrameTime.PhysicsTime = frameTimeRecord.PhysicsTime;
            sample.FrameTime.AnimationTime = frameTimeRecord.AnimationTime;
            sample.FrameTime.CpuTime = frameTimeRecord.CPUFrameTime;
            sample.FrameTime.GpuTime = frameTimeRecord.GPUFrameTime;

            sample.FrameIndex = Time.frameCount;

            m_Session.Samples.Add(sample);
        }

        public override void DoEnd()
        {
            LogRecord logRecord = (LogRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Log);
            //m_Sender.AddNetData(m_Session, logRecord.FilePath);

            SendToServer(m_Session, logRecord.FilePath);
        }

        public override void DoDispose()
        {
            
        }

        private async void SendToServer(GPerfSession session,string logPath)
        {
            var result = await HttpClientUtil.PostAsync(m_URL, session.ToByteArray(), "application/x-protobuf");

            Debug.Log("SSSSSSSSS++++" + result);

            if(!string.IsNullOrEmpty(result))
            {
                try
                {
                    JObject resultJsonObj = JObject.Parse(result);

                    JToken messageToken = resultJsonObj["message"];
                    if(messageToken!=null && messageToken.Value<string>() == "ok")
                    {
                        string logUrl = resultJsonObj["url"].Value<string>();
                        string method = resultJsonObj["method"].Value<string>();
                        string gzip = resultJsonObj["gzip"].Value<string>();

                        

                    }
                }catch
                {

                }
            }
        }
    }
}
