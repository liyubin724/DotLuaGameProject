using Google.Protobuf;
using Gperf.U3D;
using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
            var result = await RemoteUtil.PostSessionAsync(m_URL, session.ToByteArray(), "application/x-protobuf");

            /*{"message":"OK",
             * "uploadLogs":{
             *  "url":"http://hb.ix2.cn:8008/rog2/s3logs/client/1970/1/19/580fe550929721ef0c93f1e089f5c8fa.gz?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Date=20201027T103655Z&X-Amz-SignedHeaders=content-type%3Bhost%3Bx-amz-acl&X-Amz-Expires=600&X-Amz-Credential=test-key%2F20201027%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Signature=a82e7cb10e469ea54da0fea1013878d9129777e3e60f55b28b9ca7951d331afa",
             *  "method":"PUT",
             *  "headers":{"x-amz-acl":"public-read","host":"hb.ix2.cn:8008","content-type":"application/gzip"},
             *  "compression":"GZIP",
             *  "expires":1603795615}}
            */
            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(logPath) && File.Exists(logPath))
            {
                Debug.Log($"{GPerfUtil.LOG_NAME}::SendToServer->Send Log to Server.result = {result}");
                SendLogToServer(result,logPath);
            }
        }

        private async void SendLogToServer(string result,string logPath)
        {
            try
            {
                JObject resultJsonObj = JObject.Parse(result);

                JToken messageToken = resultJsonObj["message"];
                if (messageToken != null && messageToken.Value<string>() == "OK")
                {
                    var uploadLogInfo = resultJsonObj["uploadLogs"];
                    if (uploadLogInfo != null)
                    {
                        var logBytes = await RemoteUtil.GZipFileAsync(logPath);
                        if(logBytes!=null && logBytes.Length>0)
                        {
                            var headerInfo = uploadLogInfo["headers"];

                            var url = uploadLogInfo["url"].Value<string>();
                            var method = uploadLogInfo["method"].Value<string>();
                            var compression = uploadLogInfo["compression"].Value<string>();

                            Dictionary<string, string> headerDic = new Dictionary<string, string>();
                            var header = headerInfo.Next;
                            while(header != null)
                            {
                                headerDic.Add(header.Path, header.Value<string>());
                            }

                            var response = await RemoteUtil.PutLogAsync(url, logBytes, 5, headerDic);
                            Debug.Log("SSSS+++++" + response);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError("RemoteRecorder::SendLogToServer->" + e.Message);
            }
        }
    }
}
