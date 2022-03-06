using DotEngine.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPrint : MonoBehaviour
{
    
    void Start()
    {
        LogManager.CreateMgr();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Info"))
        {
            LogUtil.Info("Test-Log", "Print Info Message For Test");
        }
        if(GUILayout.Button("Warning"))
        {
            LogUtil.Warning("Test-Log", "Print Info Message For Test");
        }
        if(GUILayout.Button("Error"))
        {
            LogUtil.Error("Test-Log", "Print Info Message For Test");
        }
    }
}
