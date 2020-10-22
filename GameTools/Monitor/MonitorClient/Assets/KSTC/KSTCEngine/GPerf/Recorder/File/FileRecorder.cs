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
        public FileRecorder()
        {
            Type = RecorderType.File;
        }

        public override void DoStart()
        {
            string rootDir = GPerfUtil.GeRootDir();

            if(!string.IsNullOrEmpty(rootDir))
            {
                string[] files = Directory.GetFiles(rootDir, "gperf_*.log", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }

                try
                {
                    DateTime time = DateTime.Now;
                    string logPath = $"{rootDir}gperf_{time.Year}{time.Month}{time.Day}.log";
                    m_Writer = new StreamWriter(logPath, true, Encoding.UTF8);
                    m_Writer.AutoFlush = true;
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
