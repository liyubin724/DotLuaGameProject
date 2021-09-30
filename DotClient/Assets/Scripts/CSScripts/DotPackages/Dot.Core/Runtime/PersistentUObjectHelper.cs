using UnityEngine;

namespace DotEngine.Core
{
    public static class PersistentUObjectHelper
    {
        private const string ROOT_OBJECT_NAME = "Persistent UObject Root";
        private static Transform rootTransform = null;

        private static Transform RootTransform
        {
            get
            {
                if(rootTransform == null)
                {
                    GameObject rootGO = GameObject.Find(ROOT_OBJECT_NAME);
                    if (rootGO == null)
                    {
                        rootGO = new GameObject(ROOT_OBJECT_NAME);
                    }
                    Object.DontDestroyOnLoad(rootGO);
                    rootTransform = rootGO.transform;
                }

                return rootTransform;
            }
        }

        public static void AddGameObject(GameObject gObj)
        {
            gObj.transform.SetParent(RootTransform, false);
        }

        public static GameObject CreateGameObject(string name)
        {
            GameObject gObject = new GameObject(name);
            gObject.transform.SetParent(RootTransform, false);

            return gObject;
        }

        public static void AddTransform(Transform transform, bool worldPositionStays = false)
        {
            transform.SetParent(RootTransform, worldPositionStays);
        }

        public static Transform CreateTransform(string transformName)
        {
            GameObject gObject = new GameObject(transformName);
            Transform transform = gObject.transform;
            transform.SetParent(RootTransform, false);
            return transform;
        }

        public static T CreateComponent<T>(string name = null) where T : MonoBehaviour
        {
            T component = RootTransform.GetComponentInChildren<T>();
            if(component == null)
            {
                GameObject gObj = CreateGameObject(string.IsNullOrEmpty(name) ? typeof(T).Name : name);
                component = gObj.AddComponent<T>();
            }

            return component;
        }

        public static void Destroy()
        {
            if(rootTransform!=null)
            {
                Object.Destroy(rootTransform.gameObject);
                rootTransform = null;
            }
        }
    }
}