using DotEngine.PMonitor.Sampler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEngine.PMonitor.Recorder
{
    public class FileRecorder : ARecorder
    {
        private Dictionary<SamplerCategory, StreamWriter> m_CategoryStreams = new Dictionary<SamplerCategory, StreamWriter>();

        private string m_RootDir = null;
        public FileRecorder(string rootDir) : base(RecorderCategory.File)
        {
            if(string.IsNullOrEmpty(rootDir))
            {
                m_RootDir = "./";
            }else
            {
                m_RootDir = rootDir.Replace("\\", "/");
                if(!m_RootDir.EndsWith("/"))
                {
                    m_RootDir += "/";
                }
            }
        }

        public override void Init()
        {
            if (!Directory.Exists(m_RootDir))
            {
                Directory.CreateDirectory(m_RootDir);
            }
            else
            {
                DateTime time = DateTime.Now;
                string fileStart = $"log_{time.Year}{time.Month}{time.Day}_";
                string[] files = Directory.GetFiles(m_RootDir, "*.txt", SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (!Path.GetFileName(file).StartsWith(fileStart))
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            foreach(var swKVP in m_CategoryStreams)
            {
                swKVP.Value.Flush();
                swKVP.Value.Close();
            }
            m_CategoryStreams.Clear();
        }

        public override void HandleRecord(SamplerCategory category, Record[] records)
        {
            if(!m_CategoryStreams.TryGetValue(category,out StreamWriter writer))
            {
                DateTime time = DateTime.Now;
                string logPath = $"{m_RootDir}log_{time.Year}{time.Month}{time.Day}_{category.ToString().ToLower()}.txt";
                writer = new StreamWriter(logPath,true,Encoding.UTF8);
                writer.AutoFlush = true;

                m_CategoryStreams[category] = writer;
            }

            if(records!=null && records.Length>0)
            {
                foreach(var record in records)
                {
                    string json = JsonConvert.SerializeObject(record, Formatting.None);
                    writer.WriteLine(json);
                }
            }
        }
    }
}
