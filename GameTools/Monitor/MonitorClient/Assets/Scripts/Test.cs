﻿using KSTCEngine.GPerf;
using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;

        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Battery);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.FPS);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.SystemMemory);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.ProfilerMemory);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Device);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.App);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.CPU);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.FrameTime);
        GPerfMonitor.GetInstance().OpenSampler(SamplerMetricType.Log);

        GPerfMonitor.GetInstance().OpenRecorder(RecorderType.Remote);
        GPerfMonitor.GetInstance().OpenRecorder(RecorderType.Console);
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
                    

                }
                m_IsRunning = !m_IsRunning;
            }

            if(GUILayout.Button("PrintLog"))
            {
                Debug.Log("Test LOg");
            }
        }
        GUILayout.EndArea();
    }

}
