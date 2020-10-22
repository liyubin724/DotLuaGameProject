using Google.Protobuf;
using Gperf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace KSTCEngine.GPerf
{
    internal class GPerfNetData
    {
        internal GPerfSession session;
        internal string logPath;
    }

    public class GPerfBehaviour : MonoBehaviour
    {
        private List<GPerfNetData> m_NetDatas = new List<GPerfNetData>();
        private string m_NetUrl = "https://pirates-dev-api.shiyou.kingsoft.com:8443/gperf/api/UploadStats";
        private bool m_IsSending = false;

        public void SetNetUrl(string url)
        {
            m_NetUrl = url;
        }

        public void AddNetData(GPerfSession session,string log)
        {
            m_NetDatas.Add(new GPerfNetData() { session = session, logPath = log });
        }

        IEnumerator PostMessage(GPerfNetData netData)
        {
            UnityWebRequest request = new UnityWebRequest(m_NetUrl, "POST");
            request.uploadHandler = new UploadHandlerRaw(netData.session.ToByteArray());
            request.uploadHandler.contentType = "application/x-protobuf";
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = 2000;

            yield return request.SendWebRequest();

            m_IsSending = false;
            if(request.responseCode == 200)
            {
                string jsonText = request.downloadHandler.text;
                Debug.Log("SendSuccess->" + jsonText);
            }else
            {
                Debug.LogError($"GPerfBehaviour::PostMessage->request failed.code = {request.responseCode}");
            }
        }

        private void Update()
        {
            GPerfMonitor.GetInstance().DoUpdate(Time.deltaTime);

            if(!m_IsSending&&m_NetDatas.Count>0)
            {
                GPerfNetData data = m_NetDatas[0];
                m_NetDatas.RemoveAt(0);
                m_IsSending = true;
                StartCoroutine(PostMessage(data));
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            GPerfMonitor.GetInstance().DoDispose();

        }
    }
}
