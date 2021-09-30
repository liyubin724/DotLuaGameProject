using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public static class PoolUtill
    {
        internal static readonly string LOG_TAG = "GOPool";
        private static readonly string ROOT_NAME = "GOP-Root";

        public static Func<string, UnityObject, UnityObject> InstantiateAsset { get; set; }
        public static bool IsInDebug { get; set; } = true;

        private static Transform rootTransform = null;
        public static Transform Root
        {
            get
            {
                if (rootTransform == null)
                {
                    GameObject gObj = new GameObject(ROOT_NAME);
                    rootTransform = gObj.transform;

                    UnityObject.DontDestroyOnLoad(gObj);
                }
                return rootTransform;
            }
        }

        internal static bool IsNull(this UnityObject obj)
        {
            if (obj == null || obj.Equals(null))
            {
                return true;
            }
            return false;
        }
    }
}
