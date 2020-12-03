using DotEngine.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine
{
    public class CoroutineRunner : MonoBehaviour
    {
        private const string NAME = "Coroutine Handler";

        private static CoroutineRunner sm_Handler;
        private static CoroutineRunner GetRunner()
        {
            if(sm_Handler == null)
            {
                sm_Handler = DontDestroyUtility.CreateComponent<CoroutineRunner>(NAME);
            }
            return sm_Handler;
        }

        private Dictionary<string, List<IEnumerator>> routineDic = new Dictionary<string, List<IEnumerator>>();

        public static void Start(string tag, IEnumerator routine)
        {
            IEnumerator t = GetRunner().Start2(tag, routine);

            GetRunner().StartCoroutine(t);
        }

        public static void Stop(IEnumerator routine)
        {
            GetRunner().StopCoroutine(routine);
        }

        public static void StopAll(string tag)
        {

        }

        private IEnumerator Start2(string tag,IEnumerator routine)
        {
            Debug.Log("ssssssssss====1" + tag+",Frame = "+Time.frameCount);
            yield return null;
            Debug.Log("ssssssssss====2" + tag + ",Frame = " + Time.frameCount);
            StartCoroutine(routine);
            Debug.Log("ssssssssss====3" + tag + ",Frame = " + Time.frameCount);
            yield return null;
            Debug.Log("ssssssssss====4" + tag + ",Frame = " + Time.frameCount);
        }
    }
}
