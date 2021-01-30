using UnityEngine;

namespace DotEngine.Utilities
{
    public class DontDestoryBehaviour : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyUtility.AddTransform(transform);
            Destroy(this);
        }
    }
}