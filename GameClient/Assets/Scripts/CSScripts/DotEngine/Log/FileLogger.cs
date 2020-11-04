using System;
using System.IO;
using UnityEngine;

namespace DotEngine.Log
{
    public class FileLogger : Logger
    {
        private StreamWriter m_Writer = null;
        public FileLogger()
        {
            string dirPath = null;
#if UNITY_EDITOR
            dirPath = Application.dataPath + "/../Log";
#endif
            if(!Directory.Exists(dirPath))
            {
                DirectoryInfo dInfo = Directory.CreateDirectory(dirPath);
                if(dInfo == null || !dInfo.Exists)
                {
                    dirPath = null;
                }
            }

            if(!string.IsNullOrEmpty(dirPath))
            {
                try
                {
                    m_Writer = new StreamWriter($"{dirPath}/log-{DateTime.Now.ToString("yy-MM-dd")}.txt");
                    m_Writer.AutoFlush = true;
                }catch
                {
                    m_Writer?.Close();
                    m_Writer = null;
                }
            }
        }

        public override void Close()
        {
            m_Writer?.Flush();
            m_Writer?.Close();
            m_Writer = null;
        }

        public override void LogDebug(string tag, string message)
        {
            m_Writer?.WriteLine($"[{DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff")}] [Debug] [{tag}] {message}");
        }

        public override void LogError(string tag, string message)
        {
            m_Writer?.WriteLine($"[{DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff")}] [Error] [{tag}] {message}");
        }

        public override void LogFatal(string tag, string message)
        {
            m_Writer?.WriteLine($"[{DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff")}] [Fatal] [{tag}] {message}");
        }

        public override void LogInfo(string tag, string message)
        {
            m_Writer?.WriteLine($"[{DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff")}] [Info] [{tag}] {message}");
        }

        public override void LogWarning(string tag, string message)
        {
            m_Writer?.WriteLine($"[{DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff")}] [Warning] [{tag}] {message}");
        }
    }
}
