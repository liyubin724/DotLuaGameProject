﻿using Google.Protobuf;
using Gperf;
using KSTCEngine.GPerf.Sampler;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace KSTCEngine.GPerf.Recorder
{
    public class RemoteRecorder : GPerfRecorder
    {
        private DateTime m_OrgTime = new DateTime(1970, 1, 1);
        public float RecordInterval { get; set; } = 1.0f;
        
        private GPerfSession session = null;
        private float m_ElapsedTime = 0.0f;

        public RemoteRecorder()
        {
            Type = RecorderType.Remote;
        }

        public override void DoStart()
        {
            session = new GPerfSession();

            session.Device = new GPerfDevice();
            DeviceRecord deviceRecord = (DeviceRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Device);
            session.Device.Id = deviceRecord.UniqueIdentifier;
            session.Device.Model = deviceRecord.Model;

            session.App = new GPerfApp();
            session.App.Id = "com.ks.tc.monitor";
            session.App.Version = "1.0.0";
            session.App.Engine = "unity";
            session.App.EngineVersion = "8.8";
        }

        public override void DoUpdate(float deltaTime)
        {
            m_ElapsedTime += deltaTime;
            if(m_ElapsedTime >= RecordInterval)
            {
                m_ElapsedTime -= RecordInterval;

                GPerfSample sample = new GPerfSample();
                sample.Timestamp = (uint)((DateTime.Now - m_OrgTime).Ticks/ 10000000);

                FPSRecord fpsRecord = (FPSRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.FPS);
                sample.Fps = fpsRecord.FPS;

                CPURecord cpuRecord = (CPURecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.CPU);
                sample.CpuPercent = cpuRecord.UsageRate;
                sample.CpuMhz = cpuRecord.Frequency;

                MemoryRecord memoryRecord = (MemoryRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Memory);
                sample.MemPssKb = memoryRecord.PSSMem * 0.001f;

                BatteryRecord batteryRecord = (BatteryRecord)GPerfMonitor.GetInstance().GetSamplerRecord(SamplerMetricType.Battery);
                sample.Battery = new Battery();
                sample.Battery.Power = (int)(batteryRecord.Rate * 100);
                sample.Battery.Charging = batteryRecord.Status;
                sample.Battery.Temperature = batteryRecord.Temperature;

                sample.FrameCounter = Time.frameCount;

                session.Samples.Add(sample);
            }
        }

        public override void DoEnd()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp("https://pirates-dev-api.shiyou.kingsoft.com:8443/gperf/api/UploadStats");
            request.ContentType = "application/x-protobuf";
            request.Method = "POST";
            request.Timeout = 2000;

            byte[] data = session.ToByteArray();
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            response.Close();

            Debug.LogError("SSSSSSSS->" + result);
        }

        public override void HandleRecord(Record record)
        {
        }
    }
}
