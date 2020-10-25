using Google.Protobuf;
using Gperf.U3D;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace KSTCEngine.GPerf.Recorder
{
    internal class GPerfNetData
    {
        internal GPerfSession session;
        internal string logPath;
    }

    internal class RemoteSender : IDisposable
    {
        private List<GPerfNetData> m_NetDatas = new List<GPerfNetData>();
        private object m_NetLocker = new object();
        private Thread m_NetThread = null;

        private bool m_IsSending = false;
        private string m_NetUrl = "http://hb.ix2.cn:16408/u3d/uploadStats";

        internal RemoteSender()
        {
            m_NetThread = new Thread(OnThreadUpdate);
            m_NetThread.Start();
        }

        private void OnThreadUpdate()
        {
            while(true)
            {
                if(!m_IsSending)
                {
                    GPerfNetData netData = null;

                    lock (m_NetLocker)
                    {
                        if(m_NetDatas.Count>0)
                        {
                            m_IsSending = true;

                            netData = m_NetDatas[0];
                            m_NetDatas.RemoveAt(0);
                        }
                    }

                    if(netData!=null)
                    {
                        SendNetDataToServer(netData);
                    }else
                    {
                        Thread.Sleep(2000);
                    }
                }else
                {
                    Thread.Sleep(2000);
                }
            }
        }

        private void SendNetDataToServer(GPerfNetData netData)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(m_NetUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-protobuf";
            webRequest.Timeout = 2000;

            byte[] data = netData.session.ToByteArray();
            webRequest.ContentLength = data.Length;
            Stream writer = webRequest.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            if(response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
                string result = reader.ReadToEnd();
                UnityEngine.Debug.LogError("SSSSSSSS->" + result);
                JObject resutJson = JObject.Parse(result);

            }
            response.Close();

        }

        public void SendLogToServer(string url,string logPath)
        {

        }

        internal void AddNetData(GPerfSession session,string log)
        {
            lock(m_NetLocker)
            {
                m_NetDatas.Add(new GPerfNetData()
                {
                    session = session,
                    logPath = log
                });
            }
        }

        public void Dispose()
        {
            
        }
    }
}
