using DotEngine.Monitor.Sampler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEngine.Monitor.Recorder
{
    public class FileRecorder : MonitorRecorder
    {
        public override MonitorRecorderType Type => MonitorRecorderType.File;
        
        private Dictionary<MonitorSamplerType, StreamWriter> m_SamplerStreams = new Dictionary<MonitorSamplerType, StreamWriter>();
        private string m_RootDir = null;

        public FileRecorder(string rootDir)
        {
            m_RootDir = rootDir.Replace("\\", "/");
            if (!m_RootDir.EndsWith("/"))
            {
                m_RootDir += "/";
            }
        }

        public override void DoInit()
        {
            if (!Directory.Exists(m_RootDir))
            {
                Directory.CreateDirectory(m_RootDir);
            }
            else
            {
                DateTime time = DateTime.Now;
                string fileStart = $"log_{time.Year}{time.Month}{time.Day}_";
                string[] files = Directory.GetFiles(m_RootDir, "*.txt", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (!Path.GetFileName(file).StartsWith(fileStart))
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
        }

        public override void HandleRecords(MonitorSamplerType type, MonitorRecord[] records)
        {
            if (!m_SamplerStreams.TryGetValue(type, out StreamWriter writer))
            {
                DateTime time = DateTime.Now;
                string logPath = $"{m_RootDir}log_{time.Year}{time.Month}{time.Day}_{type.ToString().ToLower()}.log";
                writer = new StreamWriter(logPath, true, Encoding.UTF8);
                writer.AutoFlush = true;

                m_SamplerStreams[type] = writer;
            }

            if (records != null && records.Length > 0)
            {
                foreach (var record in records)
                {
                    string json = JsonConvert.SerializeObject(record, Formatting.None);
                    writer.WriteLine(json);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach(var kvp in m_SamplerStreams)
            {
                kvp.Value.Flush();
                kvp.Value.Close();
            }
            m_SamplerStreams.Clear();
        }
    }
}
