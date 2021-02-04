using UnityEngine;

namespace DotEngine.GOP
{
    public class GOPoolItem : MonoBehaviour
    {
        public string PoolName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;

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
            if (string.IsNullOrEmpty(PoolName) || string.IsNullOrEmpty(GroupName))
            {
                Destroy(CachedGameObject);
                return;
            }

            GOPoolManager poolService = Facade.GetInstance().GetServicer<GOPoolManager>(GOPoolManager.NAME);

            if (!poolService.HasGroup(GroupName))
            {
                Destroy(CachedGameObject);
                return;
            }
            GOPoolGroup spawnPool = poolService.GetGroup(GroupName);
            GOPool gObjPool = spawnPool.GetPool(PoolName);
            if (gObjPool == null)
            {
                Destroy(CachedGameObject);
                return;
            }

            gObjPool.ReleaseItem(CachedGameObject);
        }
    }
}
