using UnityEngine;

namespace KSTCEngine.GPerf
{
    public class GPerfBehaviour : MonoBehaviour
    {
        private void Update()
        {
            GPerfMonitor.GetInstance().DoUpdate(Time.unscaledDeltaTime);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            GPerfMonitor.GetInstance().DoDispose();
        }


        //private List<GPerfNetData> m_NetDatas = new List<GPerfNetData>();
        //private string m_NetUrl = "http://hb.ix2.cn:16408/u3d/uploadStats";
        //private bool m_IsSending = false;
        //private bool m_IsUploadLog = false;

        //public void SetData(string url,bool isUploadLog)
        //{
        //    m_NetUrl = url;
        //    m_IsUploadLog = isUploadLog;
        //}

        //public void AddNetData(GPerfSession session,string log)
        //{
        //    m_NetDatas.Add(new GPerfNetData() { session = session, logPath = log });
        //}

        //IEnumerator PostMessage(GPerfNetData netData)
        //{
        //    UnityWebRequest request = new UnityWebRequest(m_NetUrl, "POST");
        //    request.uploadHandler = new UploadHandlerRaw(netData.session.ToByteArray());
        //    request.uploadHandler.contentType = "application/x-protobuf";
        //    request.downloadHandler = new DownloadHandlerBuffer();
        //    request.timeout = 2000;

        //    yield return request.SendWebRequest();

        //    m_IsSending = false;
        //    if(request.responseCode == 200)
        //    {
        //        if(m_IsUploadLog)
        //        {
        //            string jsonText = request.downloadHandler.text;
        //            Debug.Log("SendSuccess->" + jsonText);

        //        }
        //    }else
        //    {
        //        Debug.LogError($"GPerfBehaviour::PostMessage->request failed.code = {request.responseCode}");
        //    }
        //}

        //private void Update()
        //{
        //    GPerfMonitor.GetInstance().DoUpdate(Time.unscaledDeltaTime);

        //    if(!m_IsSending&&m_NetDatas.Count>0)
        //    {
        //        GPerfNetData data = m_NetDatas[0];
        //        m_NetDatas.RemoveAt(0);
        //        m_IsSending = true;
        //        StartCoroutine(PostMessage(data));
        //    }
        //}

        //private void OnDestroy()
        //{
        //    StopAllCoroutines();
        //    GPerfMonitor.GetInstance().DoDispose();

        //}
    }
}
