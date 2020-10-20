using KSTCEngine.GPerf;
using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private bool m_IsRunning = false;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            if(GUILayout.Button(m_IsRunning?"Stop":"Start",GUILayout.Height(40)))
            {
                if(m_IsRunning)
                {
                    GPerfMonitor.GetInstance().Shuntdown();
                }
                else
                {
                    GPerfMonitor.GetInstance().Startup();
                    GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Battery);
                    GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.FPS);
                    GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Memory);
                    GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Device);
                    GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.CPU);

                    GPerfMonitor.GetInstance().OpenRecorder(RecorderType.Remote);
                    GPerfMonitor.GetInstance().OpenRecorder(RecorderType.Console);

                }
                m_IsRunning = !m_IsRunning;
            }
        }
        GUILayout.EndArea();
    }

}
