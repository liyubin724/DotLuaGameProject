using System;
using UnityEngine;

namespace DotEngine.Core
{
    public class UpdateBehaviour : SingletonBehaviour<UpdateBehaviour>
    {
        public event Action<float, float> updateEvent;
        public event Action<float, float> lateUpdateEvent;
        public event Action<float, float> fixedUpdateEvent;

        void Update()
        {
            updateEvent?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            lateUpdateEvent?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void FixedUpdate()
        {
            fixedUpdateEvent?.Invoke(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }

        protected override void OnDestroy()
        {
            updateEvent = null;
            lateUpdateEvent = null;
            fixedUpdateEvent = null;

            base.OnDestroy();
        }
    }
}
