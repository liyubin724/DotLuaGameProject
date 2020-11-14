using System;
using UnityEngine;

namespace DotEngine
{
    public class UpdateBehaviour : MonoBehaviour
    {
        public const string NAME = "Update-Root";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnStartup()
        {
            GameObject gObj = new GameObject(NAME);
            gObj.AddComponent<UpdateBehaviour>();
            DontDestroyOnLoad(gObj);
        }

        public static UpdateBehaviour Updater { get; private set; } = null;

        public event Action<float,float> OnUpdate;
        public event Action<float,float> OnLateUpdate;
        public event Action<float,float> OnFixedUpdate;

        private void Awake()
        {
            if(Updater!=null)
            {
                Destroy(this);
            }else
            {
                Updater = this;
            }
        }

        private void Update()
        {
            OnUpdate?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }

        private void OnDestroy()
        {
            
        }
    }
}
