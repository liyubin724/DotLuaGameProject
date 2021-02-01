using DotEngine.Log;
using DotEngine.Log.Appender;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    void Start()
    {
        LogUtil.AddAppender(new ServerLogSocketAppender());
        LogUtil.AddAppender(new UnityConsoleAppender());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Log Info",GUILayout.Height(80),GUILayout.ExpandWidth(true)))
        {
            LogUtil.Info("Log Info message");
        }
        if(GUILayout.Button("Log Warning", GUILayout.Height(80), GUILayout.ExpandWidth(true)))
        {
            LogUtil.Warning("Log Warning Message");
        }
        if(GUILayout.Button("Log Error", GUILayout.Height(80), GUILayout.ExpandWidth(true)))
        {
            LogUtil.Error("Log Error Message");
        }
    }

    private void OnDestroy()
    {
        LogUtil.Reset();
    }
}
