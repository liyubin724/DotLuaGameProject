using DotEngine.Log.Formatter;
using System;
using System.IO;
using UnityEngine;

namespace DotEngine.Log.Appender
{
    public class FileLogAppender : ALogAppender
    {
        public static readonly string NAME = "FileLog";

        private StreamWriter m_Writer = null;

        public FileLogAppender(ILogFormatter formatter) : base(NAME, formatter)
        {
            string dirPath = null;
#if UNITY_EDITOR
            dirPath = Application.dataPath + "/../Log";
#endif
            if (!Directory.Exists(dirPath))
            {
                DirectoryInfo dInfo = Directory.CreateDirectory(dirPath);
                if (dInfo == null || !dInfo.Exists)
                {
                    dirPath = null;
                }
            }

            if (!string.IsNullOrEmpty(dirPath))
            {
                try
                {
                    m_Writer = new StreamWriter($"{dirPath}/log-{DateTime.Now.ToString("yy-MM-dd")}.txt");
                    m_Writer.AutoFlush = true;
                }
                catch
                {
                    m_Writer?.Close();
                    m_Writer = null;
                }
            }

        }

        public FileLogAppender() : this(new DefaultLogFormatter())
        {
        }

        protected override void DoLogMessage(LogLevel level, string message)
        {
            m_Writer?.WriteLine(message);
        }

        protected override void OnDisposed()
        {
            m_Writer?.Flush();
            m_Writer?.Close();
            m_Writer = null;
        }
    }
}
