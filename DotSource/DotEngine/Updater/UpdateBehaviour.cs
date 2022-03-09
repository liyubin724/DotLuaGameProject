using System;
using UnityEngine;

namespace DotEngine.Updater
{
    public class UpdateBehaviour : MonoBehaviour
    {
        private Action<float, float> updateHandler;
        private Action<float, float> lateUpdateHandler;
        private Action<float, float> fixedUpdateHandler;

        internal void SetHandler(Action<float,float> updater,Action<float,float> lateUpdater,Action<float,float> fixedUpdater)
        {
            updateHandler = updater;
            lateUpdateHandler = lateUpdater;
            fixedUpdateHandler = fixedUpdater;
        }

        void Update()
        {
            updateHandler?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            lateUpdateHandler?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void FixedUpdate()
        {
            fixedUpdateHandler?.Invoke(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }
    }
}
