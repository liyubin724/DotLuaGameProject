﻿using ICSharpCode.SharpZipLib.GZip;
using KSTCEngine.GPerf;
using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        //GPerfMonitor.GetInstance().OpenRecorder(RecorderType.File);

        //GPerfPlatform.InitPlugin();
        //string values = "Test for Today";
        //byte[] bytes = Encoding.UTF8.GetBytes(values);
        //MemoryStream orgMS = new MemoryStream(bytes);

        //MemoryStream outMS = new MemoryStream();
        //GZip.Compress(orgMS, outMS, false);
        //byte[] rBytes = outMS.ToArray();
        //orgMS.Close();
        //outMS.Close();

        //File.WriteAllBytes("D:/t.bytes", rBytes);

        //byte[] cBytes = File.ReadAllBytes("D:/t.bytes");
        //using(MemoryStream cInMS = new MemoryStream(cBytes))
        //{
        //    using(MemoryStream oInMS = new MemoryStream())
        //    {
        //        GZip.Decompress(cInMS, oInMS, false);
        //        byte[] r = oInMS.ToArray();

        //        var rs = Encoding.UTF8.GetString(r);
        //        Debug.Log("SSSSSSSS+" + rs);
        //    }
        //}

    }

    private bool m_IsRunning = false;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            if (GUILayout.Button(m_IsRunning ? "Stop" : "Start", GUILayout.Height(40)))
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

            if (GUILayout.Button("PrintLog"))
            {
                Debug.Log("Test LOg");
            }
        }
        GUILayout.EndArea();
    }

}
