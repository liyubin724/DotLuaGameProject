using System;
using UnityEngine;

namespace DotEngine.Updater
{
    public class UpdateBehaviour : MonoBehaviour
    {
        public Action<float, float> Handler;
        
        void Update()
        {
            Handler?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
}
