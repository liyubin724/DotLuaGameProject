using DotEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator ie = Print();
        CoroutineRunner.Start("Test", ie);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Print()
    {
        Debug.Log("DDDDDDDDDDDDDD1" + ",Frame = " + Time.frameCount);
        yield return null;

        Debug.Log("SDDDDDDDDDDDDD2" + ",Frame = " + Time.frameCount);
        yield return new WaitForSeconds(0.2f);

        Debug.Log("SDDDDDDDDDDDDD3" + ",Frame = " + Time.frameCount);
    }
}
