using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FPSProfiler : EditorWindow
{
    [MenuItem("Test/Profiler")]
    static void ShowWin()
    {
        FPSProfiler.GetWindow<FPSProfiler>().Show();
    }

    private void OnGUI()
    {
        if(Event.current.type == EventType.Repaint)
        {
            //GUI.BeginClip(new Rect(100, 100, 100, 100));
            {
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(2.5f, new Vector3(0, 0, 0), new Vector3(0, 100, 0), new Vector3(100, 100, 0));

                
            }
            //GUI.EndClip();
        }

        
    }
}
