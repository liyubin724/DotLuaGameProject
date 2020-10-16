using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Recorder
{
    public class FileRecorder : GPerfRecorder
    {
        public override RecorderType Type => RecorderType.File;

        private Dictionary<SamplerType, StreamWriter> m_SamplerStreams = new Dictionary<SamplerType, StreamWriter>();
        private string m_RootDir = null;

        public FileRecorder()
        {
        }

        private string GetRootDir()
        {
#if UNITY_EDITOR
            return Application.dataPath.Replace("Assets", "GPerf/");
#else
#if UNITY_ANDROID
        return Application.persistentDataPath+"/GPerf/";
#elif UNITY_IPHONE

#endif
#endif
        }

        public override void DoInit()
        {
            m_RootDir = GetRootDir();

            if (!Directory.Exists(m_RootDir))
            {
                Directory.CreateDirectory(m_RootDir);
            }
            else
            {
                string[] files = Directory.GetFiles(m_RootDir, "*.log", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public override void HandleRecords(Record[] records)
        {
            if (records != null && records.Length > 0)
            {
                foreach (var record in records)
                {
                    if (!string.IsNullOrEmpty(record.Data))
                    {
                        WriteRecord(record);
                    }
                }
            }
        }

        private void WriteRecord(Record record)
        {
            if (!m_SamplerStreams.TryGetValue(record.Type, out StreamWriter writer))
            {
                DateTime time = DateTime.Now;
                string logPath = $"{m_RootDir}log_{time.Year}{time.Month}{time.Day}_{record.Type.ToString().ToLower()}.log";
                writer = new StreamWriter(logPath, true, Encoding.UTF8);
                writer.AutoFlush = true;

                m_SamplerStreams[record.Type] = writer;
            }

            string json = JsonConvert.SerializeObject(record, Formatting.Indented);
            writer.WriteLine(json);
        }

        public override void DoDispose()
        {
            foreach (var kvp in m_SamplerStreams)
            {
                kvp.Value.Flush();
                kvp.Value.Close();
            }
            m_SamplerStreams.Clear();

            base.DoDispose();
        }
    }
}
