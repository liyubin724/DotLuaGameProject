using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataRun : MonoBehaviour
{
    TestData data1 = null;
    TestData data2 = null;
    void Start()
    {
        data1 = Resources.Load<TestData>("test_data");
        data2  = Resources.Load<TestData>("test_data");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Change Data1"))
        {
            data1.intValue = 100;
        }

        if(GUILayout.Button("Print Data2"))
        {
            Debug.Log("" + data2.intValue);
        }
    }
}
