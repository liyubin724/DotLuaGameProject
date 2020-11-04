using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

        private List<string> m_CachedMessage = new List<string>();
        private Thread m_WriterThread = null;
        private object m_Locker = new object();
        private CancellationTokenSource m_TokenSource = new CancellationTokenSource();
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

                    m_WriterThread = new Thread(() =>
                    {
                        while (true)
                        {
                            if(m_TokenSource.Token.IsCancellationRequested)
                            {
                                break;
                            }

                            lock (m_Locker)
                            {
                                if (m_CachedMessage.Count > 0)
                                {
                                    foreach (var line in m_CachedMessage)
                                    {
                                        m_Writer.WriteLine(line);
                                    }
                                    m_CachedMessage.Clear();
                                }
                            }
                            Thread.Sleep(2000);
                        }
                    });
                    m_WriterThread.IsBackground = true;
                    m_WriterThread.Start();
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
            m_TokenSource.Cancel();
            m_WriterThread = null;
            m_Writer?.Flush();
            m_Writer?.Close();
            m_Writer = null;

            m_CachedMessage.Clear();
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
            if (condition.IndexOf(GPerfUtil.LOG_NAME) >= 0 || m_Writer == null)
            {
                return;
            }

            lock (m_Locker)
            {
                m_CachedMessage.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff} {type.ToString()}\n{condition}\n{stackTrace}");
            }
        }
    }
}
