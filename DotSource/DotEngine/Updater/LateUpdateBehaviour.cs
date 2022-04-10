using System;
using UnityEngine;

namespace DotEngine.Updater
{
    public class LateUpdateBehaviour : MonoBehaviour
    {
        public Action<float, float> Handler;

        void LateUpdate()
        {
            Handler?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
}
