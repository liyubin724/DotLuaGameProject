using System;
using UnityEngine;

namespace DotEngine.Framework.Updater
{
    public class UpdaterBehaviour : MonoBehaviour
    {
        public event Action<float, float> UpdateAction;
        public event Action<float, float> LateUpdateAction;
        public event Action<float,float> FixedUpdateAction;

        private static UpdaterBehaviour updaterBeh = null;
        public static UpdaterBehaviour GetUpdater()
        {
            if(updaterBeh == null)
            {
                GameObject go = new GameObject("Updater Root");
                DontDestroyOnLoad(go);

                updaterBeh = go.AddComponent<UpdaterBehaviour>();
            }
            return updaterBeh;
        }

        private void Update()
        {
            UpdateAction?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            LateUpdateAction?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            FixedUpdateAction?.Invoke(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }
    }
}
