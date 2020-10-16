using KSTCEngine.GPerf;
using KSTCEngine.GPerf.Recorder;
using KSTCEngine.GPerf.Sampler;
using System.Text;
using UnityEngine;

public class Test : MonoBehaviour
{
    private string content = string.Empty;
    void Start()
    {
        Application.targetFrameRate = 60;
        GPerfMonitor.Startup();

        //AndroidJavaClass ajc = new AndroidJavaClass("com.kingsoft.tc.uplugin.util.ProcCPUStatInfoUtil");
        //content = ajc.CallStatic<string>("getUsageRateByCmd");

    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
    //    {
    //        GUILayout.TextArea(content, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
    //    }
    //    GUILayout.EndArea();
    //}

}
