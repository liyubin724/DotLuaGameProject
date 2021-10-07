using UnityEngine;

namespace DotEngine.Core
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        private static T instance = null;
        public static T GetInstance()
        {
            if (instance == null)
            {
                instance = PersistentUObjectHelper.CreateComponent<T>();
            }
            return instance;
        }

        public static void DestroyInstance()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
                instance = null;
            }
        }

        protected virtual void Awake()
        {
            if(instance == null)
            {
                instance = (T)this;
            }else if(instance!=null)
            {
                if(instance != this)
                {
                    Debug.LogError("The instance has been created!!");
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance != null)
            {
                instance = null;
            }
        }
    }
}
