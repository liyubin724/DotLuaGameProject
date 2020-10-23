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
        private int m_SamplingCount = 0;
        private string m_RootDir = string.Empty;

        public LogSampler()
        {
            MetricType = SamplerMetricType.Log;
            FreqType = SamplerFreqType.End;

            m_RootDir = GPerfUtil.GeRootDir();

            if (!string.IsNullOrEmpty(m_RootDir))
            {
                string[] files = Directory.GetFiles(m_RootDir, "log_*.log", SearchOption.TopDirectoryOnly);
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

            if (!string.IsNullOrEmpty(m_RootDir))
            {
                try
                {
                    DateTime time = DateTime.Now;
                    string logPath = $"{m_RootDir}log_{time.Year}{time.Month}{time.Day}_{m_SamplingCount}.log";
                    m_Writer = new StreamWriter(logPath, true, Encoding.UTF8);
                    m_Writer.AutoFlush = true;

                    record.FilePath = logPath;
                    m_SamplingCount++;
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
