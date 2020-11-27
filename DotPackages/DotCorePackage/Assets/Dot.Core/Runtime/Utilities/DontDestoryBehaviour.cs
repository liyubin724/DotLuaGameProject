using UnityEngine;

namespace DotEngine.Utilities
{
    public class DontDestoryBehaviour : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyHandler.AddTransform(transform);
            Destroy(this);
        }
    }
}