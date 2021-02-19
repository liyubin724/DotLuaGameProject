using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DotEngine.Log
{
    public class FileLogAppender : ALogAppender
    {
        private string outputDir = string.Empty;
        private StreamWriter fileWriter = null;

        private List<string> cachedLogs = new List<string>();
        private Thread fileWriterThread = null;
        private object locker = new object();
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public FileLogAppender(string logFileDir, ILogFormatter formatter) : base(typeof(FileLogAppender).Name, formatter)
        {
            outputDir = logFileDir;
        }

        public FileLogAppender(string logFileDir) : this(logFileDir,new DefaultLogFormatter())
        {
        }

        public override void OnAppended()
        {
            if(string.IsNullOrEmpty(outputDir))
            {
                return;
            }

            string fileName = $"log-{DateTime.Now.ToString("yy-MM-dd")}.log";
            if(Directory.Exists(outputDir))
            {
                string[] files = Directory.GetFiles(outputDir, "log-*.log", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (Path.GetFileName(file) != fileName)
                        {
                            File.Delete(file);
                        }
                    }
                }
            }else
            {
                DirectoryInfo dInfo = Directory.CreateDirectory(outputDir);
                if(!dInfo.Exists)
                {
                    return;
                }
            }

            string filePath = $"{outputDir}/{fileName}";
            try
            {
                fileWriter = new StreamWriter(filePath,true,Encoding.UTF8);
                fileWriter.AutoFlush = true;

                fileWriterThread = new Thread(() =>
                {
                    while (true)
                    {
                        if (tokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        lock (locker)
                        {
                            if (cachedLogs.Count > 0)
                            {
                                foreach (var line in cachedLogs)
                                {
                                    fileWriter.WriteLine(line);
                                }
                                cachedLogs.Clear();
                            }
                        }
                        Thread.Sleep(3000);
                    }
                });
                fileWriterThread.IsBackground = true;
                fileWriterThread.Start();
            }
            catch
            {
                fileWriter?.Close();
                fileWriter = null;
            }
        }

        public override void OnRemoved()
        {
            tokenSource.Cancel();
            lock (locker)
            {
                fileWriter?.Flush();
                fileWriter?.Close();
            }
            fileWriterThread = null;
            fileWriter = null;

            cachedLogs.Clear();
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            if (fileWriter == null)
            {
                return;
            }
            lock (locker)
            {
                cachedLogs.Add(message);
            }
        }
    }
}
