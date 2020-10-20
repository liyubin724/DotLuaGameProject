using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Recorder
{
    public class FileRecorder : GPerfRecorder
    {
        private string m_RootDir = null;
        private StreamWriter m_Writer = null;
        public FileRecorder()
        {
            Type = RecorderType.File;
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

        public override void DoStart()
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

            try
            {
                DateTime time = DateTime.Now;
                string logPath = $"{m_RootDir}gperf_{time.Year}{time.Month}{time.Day}.log";
                m_Writer = new StreamWriter(logPath, true, Encoding.UTF8);
                m_Writer.AutoFlush = true;
            }catch
            {
                m_Writer?.Close();
                m_Writer = null;
            }
        }

        public override void HandleRecord(Record record)
        {
            if(m_Writer!=null)
            {
                string json = JsonConvert.SerializeObject(record, Formatting.Indented);
                m_Writer.WriteLine(json);
            }
        }

        public override void DoDispose()
        {
            m_Writer?.Close();
            m_Writer = null;

            base.DoDispose();
        }
    }
}
