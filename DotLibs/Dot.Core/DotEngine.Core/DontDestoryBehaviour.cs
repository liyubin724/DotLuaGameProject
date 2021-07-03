using UnityEngine;

namespace DotEngine.Core
{
    public class DontDestoryBehaviour : MonoBehaviour
    {
        void Awake()
        {
            PersistentUObjectHelper.AddGameObject(gameObject);
            Destroy(this);
        }
    }
}