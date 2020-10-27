using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace KSTCEngine.GPerf.Recorder
{
    public class FileRecorder : GPerfHandleRecorder
    {
        private StreamWriter m_Writer = null;
        private int m_RecordCount = 0;
        private string m_RootDir = string.Empty;

        private List<string> m_CachedMessage = new List<string>();
        private Thread m_WriterThread = null;
        private object m_Locker = new object();

        public FileRecorder():base(RecorderType.File)
        {
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

                    m_WriterThread = new Thread(() =>
                    {
                        while(true)
                        {
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
                            Thread.Sleep(3000);
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

        public override void HandleRecord(Record record)
        {
            if(m_Writer==null)
            {
                return;
            }
            lock (m_Locker)
            {
                string json = JsonConvert.SerializeObject(record, Formatting.Indented);
                m_CachedMessage.Add(json);
            }
        }

        public override void DoDispose()
        {
            if (m_WriterThread != null)
            {
                m_WriterThread.Abort();
            }
            m_WriterThread = null;

            m_Writer?.Flush();
            m_Writer?.Close();
            m_Writer = null;

            m_CachedMessage.Clear();

            base.DoDispose();
        }
    }
}
