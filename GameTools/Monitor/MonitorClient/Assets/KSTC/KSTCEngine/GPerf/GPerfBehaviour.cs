using KSTCEngine.GPerf.Recorder;
using UnityEngine;

namespace KSTCEngine.GPerf
{
    public class GPerfBehaviour : MonoBehaviour
    {
        private void Start()
        {
            GPerfMonitor.GetInstance().OpenRecorder(RecorderType.Remote);
#if GPERF_XLUA
            XLua.LuaEnv env = wt.framework.XLuaManager.Instance.GetLuaEnv();
            GPerfMonitor.GetInstance().SetLuaEnv(env);
#endif
        }

        private void Update()
        {
            GPerfMonitor.GetInstance().DoUpdate(Time.unscaledDeltaTime);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            GPerfMonitor.GetInstance().DoDispose();
        }

        private bool m_IsRunning = false;
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 60, Screen.width, Screen.height));
            {
                if (GUILayout.Button(m_IsRunning ? "Stop" : "Start", GUILayout.Height(80)))
                {
                    if (m_IsRunning)
                    {
                        GPerfMonitor.GetInstance().Shuntdown();
                    }
                    else
                    {
                        GPerfMonitor.GetInstance().Startup();
                    }
                    m_IsRunning = !m_IsRunning;
                }

                if (GUILayout.Button("PrintLog", GUILayout.Height(80)))
                {
                    Debug.Log("Test LOg");
                }
            }
            GUILayout.EndArea();
        }
    }
}
