using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace KSTCEngine.GPerf.Recorder
{
    public class FileRecorder : GPerfRecorder
    {
        private StreamWriter m_Writer = null;
        private int m_RecordCount = 0;
        private string m_RootDir = string.Empty;

        public FileRecorder()
        {
            Type = RecorderType.File;
            m_RootDir = GPerfUtil.GeRootDir();
            if (!string.IsNullOrEmpty(m_RootDir))
            {
                string[] files = Directory.GetFiles(m_RootDir, "gperf_*.log", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public override void DoStart()
        {
            if(!string.IsNullOrEmpty(m_RootDir))
            {
                try
                {
                    DateTime time = DateTime.Now;
                    string logPath = $"{m_RootDir}gperf_{time.Year}{time.Month}{time.Day}_{m_RecordCount}.log";
                    m_Writer = new StreamWriter(logPath, true, Encoding.UTF8);
                    m_Writer.AutoFlush = true;

                    m_RecordCount++;
                }
                catch
                {
                    m_Writer?.Close();
                    m_Writer = null;
                }
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
