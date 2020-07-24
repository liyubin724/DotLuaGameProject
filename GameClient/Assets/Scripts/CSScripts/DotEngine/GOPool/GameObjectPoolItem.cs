using UnityEngine;

namespace DotEngine.GOPool
{
    public class GameObjectPoolItem : MonoBehaviour
    {
        public string AssetPath { get; set; } = string.Empty;
        public string SpawnName { get; set; } = string.Empty;

        private Transform cachedTransform = null;
        public Transform CachedTransform
        {
            get {
                if (cachedTransform == null)
                {
                    cachedTransform = transform;
                }
                return cachedTransform;
            }
        }

        private GameObject cachedGameObject = null;
        public GameObject CachedGameObject
        {
            get
            {
                if(cachedGameObject == null)
                {
                    cachedGameObject = gameObject;
                }
                return cachedGameObject;
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        public virtual void DoSpawned()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void DoDespawned()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseItem()
        {
            if (string.IsNullOrEmpty(AssetPath) || string.IsNullOrEmpty(SpawnName))
            {
                Destroy(CachedGameObject);
                return;
            }

            GameObjectPoolService poolService = Facade.GetInstance().GetService<GameObjectPoolService>(GameObjectPoolService.NAME);

            if (!poolService.HasGroup(SpawnName))
            {
                Destroy(CachedGameObject);
                return;
            }
            GameObjectPoolGroup spawnPool = poolService.GetGroup(SpawnName);
            GameObjectPool gObjPool = spawnPool.GetPool(AssetPath);
            if (gObjPool == null)
            {
                Destroy(CachedGameObject);
                return;
            }

            gObjPool.ReleasePoolItem(CachedGameObject);
        }
    }
}
