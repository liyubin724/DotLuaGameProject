using Gperf.U3D;
using KSTCEngine.GPerf.Sampler;
using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Recorder
{
    public class RemoteRecorder : GPerfRecorder
    {
        private DateTime m_OrgTime;
        public float RecordInterval { get; set; } = 1.0f;
        
        private GPerfSession session = null;
        private float m_ElapsedTime = 0.0f;

        public RemoteRecorder()
        {
            m_OrgTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            Type = RecorderType.Remote;
        }

        public override void DoStart()
        {
            session = new GPerfSession();

            AppRecord appRecord = (AppRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.App);
            session.App = new GPerfApp();
            session.App.Identifier = appRecord.Identifier;
            session.App.InstallName = appRecord.InstallName;
            session.App.ProductName = appRecord.ProductName;
            session.App.Version = appRecord.Version;
            session.App.UnityVersion = appRecord.EngineVersion;


            DeviceRecord deviceRecord = (DeviceRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Device);
            session.Device = new GPerfDevice();
            session.Device.UniqueIdentifier = deviceRecord.UniqueIdentifier;
            session.Device.Model = deviceRecord.Model;

        }

        public override void DoUpdate(float deltaTime)
        {
            m_ElapsedTime += deltaTime;
            if(m_ElapsedTime >= RecordInterval)
            {
                m_ElapsedTime = 0.0f;

                GPerfSample sample = new GPerfSample();
                sample.Timestamp = (uint)((DateTime.Now - m_OrgTime).Ticks/ 10000000);

                FPSRecord fpsRecord = (FPSRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.FPS);
                sample.Fps = new GPerfFPS();
                sample.Fps.Fps = fpsRecord.FPS;
                
                CPURecord cpuRecord = (CPURecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.CPU);
                sample.Cpu = new GPerfCPU();
                sample.Cpu.CoreCount = cpuRecord.CoreCount;
                sample.Cpu.Frequency = cpuRecord.Frequency;
                sample.Cpu.UsageRate = cpuRecord.UsageRate;

                SystemMemoryRecord memoryRecord = (SystemMemoryRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.SystemMemory);
                sample.SystemMemory = new GPerfSystemMemory();
                sample.SystemMemory.TotalInKb = memoryRecord.TotalMemInKB;
                sample.SystemMemory.AvailableInKb = memoryRecord.AvailableMemInKB;
                sample.SystemMemory.ThresholdInKb = memoryRecord.ThresholdInKB;
                sample.SystemMemory.IsLow = memoryRecord.IsLowMem;
                sample.SystemMemory.PssInKb = memoryRecord.PSSMemInKB;

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

                session.Samples.Add(sample);
            }
        }

        public override void DoEnd()
        {
            LogRecord logRecord = (LogRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Log);
            GPerfMonitor.GetInstance().Behaviour.AddNetData(session, logRecord.FilePath);
        }

        public override void HandleRecord(Record record)
        {
        }
    }
}
