using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotEngine.Log
{
    public class FileLogAppender : LogAppender
    {
        public static readonly string NAME = "FileAppender";

        private int forceFlushCount = 1;
        public int ForceFlushCount
        {
            get
            {
                return forceFlushCount;
            }
            set
            {
                forceFlushCount = value >= 0 ? value : 1;
            }
        }
        private int forceFlushTime = 1;
        public int ForceFlushTime
        {
            get
            {
                return forceFlushTime;
            }
            set
            {
                forceFlushTime = value >= 0 ? value : 1;
            }
        }

        private string outputDir = string.Empty;
        private readonly ConcurrentQueue<string> cachedLogs = new ConcurrentQueue<string>();
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private string outputFileName = null;

        public FileLogAppender(string fileDir = null, ILogFormatter formatter = null) : base(NAME, formatter)
        {
            outputDir = fileDir;
        }

        ~FileLogAppender()
        {
            tokenSource.Cancel();
        }

        public override void DoInitialize()
        {
            if (!CreateOutputDir())
            {
                return;
            }
            outputFileName = $"log-{DateTime.Now.ToString("yyyy-MM-dd")}.log";
            ClearOutOfDateLogs();

            Task.Factory.StartNew(() =>
            {
                DateTime lastSaveLogTime = DateTime.Now;
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    if (cachedLogs.Count >= ForceFlushCount || (cachedLogs.Count > 0 && (DateTime.Now - lastSaveLogTime).TotalSeconds >= ForceFlushTime))
                    {
                        string[] logs = cachedLogs.ToArray();
                        File.AppendAllLines(Path.Combine(outputDir, outputFileName), logs);

                        while (cachedLogs.TryDequeue(out var item)) { }

                        lastSaveLogTime = DateTime.Now;
                    }
                    else
                    {
                        SpinWait.SpinUntil(() =>
                        {
                            return cachedLogs.Count >= ForceFlushCount;
                        }, ForceFlushTime);
                    }
                }
                if (cachedLogs.Count > 0)
                {
                    string[] logs = cachedLogs.ToArray();
                    File.AppendAllLines(Path.Combine(outputDir, outputFileName), logs);

                    while (cachedLogs.TryDequeue(out var item)) { }
                }
            }, tokenSource.Token);
        }

        private bool CreateOutputDir()
        {
            if (string.IsNullOrEmpty(outputDir))
            {
                outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            }
            else
            {
                outputDir = Path.Combine(outputDir, "Logs");
            }
            if (!Directory.Exists(outputDir))
            {
                return Directory.CreateDirectory(outputDir).Exists;
            }
            return true;
        }

        private void ClearOutOfDateLogs()
        {
            string[] files = Directory.GetFiles(outputDir, "log-*.log", SearchOption.TopDirectoryOnly);
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (Path.GetFileName(file) != outputFileName)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public override void DoDestroy()
        {
            tokenSource.Cancel();
        }

        protected override void OutputMessage(string tag, LogLevel level, string formattedMessage)
        {
            cachedLogs.Enqueue(formattedMessage);
        }
    }
}
