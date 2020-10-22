using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class LogRecord : Record
    {
        public string FilePath { get; set; } = null;
    }

    public class LogSampler : GPerfSampler<LogRecord>
    {
        private StreamWriter m_Writer = null;
        public LogSampler()
        {
            MetricType = SamplerMetricType.Log;
            FreqType = SamplerFreqType.End;

            string rootDir = GPerfUtil.GeRootDir();

            if (!string.IsNullOrEmpty(rootDir))
            {
                string[] files = Directory.GetFiles(rootDir, "log_*.log", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }
        protected override void OnStart()
        {
            Application.logMessageReceived += OnLogMessageReceived;
            string rootDir = GPerfUtil.GeRootDir();
            if (!string.IsNullOrEmpty(rootDir))
            {
                try
                {
                    DateTime time = DateTime.Now;
                    string logPath = $"{rootDir}log_{time.Year}{time.Month}{time.Day}.log";
                    m_Writer = new StreamWriter(logPath, true, Encoding.UTF8);
                    m_Writer.AutoFlush = true;

                    record.FilePath = logPath;
                }
                catch
                {
                    m_Writer?.Close();
                    m_Writer = null;
                }
            }
        }

        protected override void OnEnd()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
            m_Writer?.Flush();
            m_Writer?.Close();
            m_Writer = null;
        }

        protected override void OnSample()
        {
        }

        protected override void OnDispose()
        {
            OnEnd();
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if(condition.IndexOf(GPerfUtil.LOG_NAME)>=0 || m_Writer == null)
            {
                return;
            }

            m_Writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff} {type.ToString()}\n{condition}\n{stackTrace}");
        }
    }
}
