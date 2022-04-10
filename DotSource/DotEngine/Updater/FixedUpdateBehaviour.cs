using System;
using UnityEngine;

namespace DotEngine.Updater
{
    public class FixedUpdateBehaviour : MonoBehaviour
    {
        public Action<float, float> Handler;

        void FixedUpdate()
        {
            Handler?.Invoke(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }
    }
}
