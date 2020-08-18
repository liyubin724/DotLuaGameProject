using DotEngine.Log;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.GOPool
{
    /// <summary>
    /// 主要用于GameObjectPool的分组，可以根据使用场景进行添加和删除分组，不同的分组中可以有相同GameObject的缓存池
    /// </summary>
    public class GameObjectPoolGroup
    {
        private string name;
        private Transform parentTransform = null;
        private Transform groupTransform = null;

        private Dictionary<string, GameObjectPool> poolDic = new Dictionary<string, GameObjectPool>();

        internal GameObjectPoolGroup(string gName, Transform parentTran)
        {
            name = gName;
            parentTransform = parentTran;
#if UNITY_EDITOR
            groupTransform = new GameObject(string.Format(name)).transform;
            groupTransform.SetParent(parentTran, false);
#endif
        }

        private Transform GetTransform()
        {
            return groupTransform == null ? parentTransform : groupTransform;
        }

        public bool HasPool(string uniqueName)
        {
            return poolDic.ContainsKey(uniqueName);
        }

        /// <summary>
        /// 缓存池中将默认以资源的路径为唯一标识，通过资源唯一标识可以获致到对应的缓存池
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        public GameObjectPool GetPool(string uniqueName)
        {
            if(poolDic.TryGetValue(uniqueName,out GameObjectPool goPool))
            {
                return goPool;
            }else
            {
                LogUtil.LogWarning(GameObjectPoolConst.LOGGER_NAME, "The pool is not found.name = " + uniqueName);
            }

            return null;
        }

        /// <summary>
        /// 使用给定的GameObject创建缓存池
        /// </summary>
        /// <param name="uniqueName">资源唯一标签，一般使用资源路径</param>
        /// <param name="template">模板GameObject</param>
        /// <returns></returns>
        public GameObjectPool CreatePool(string uniqueName, PoolTemplateType templateType, GameObject template)
        {
            if(template == null)
            {
                LogUtil.LogError(GameObjectPoolConst.LOGGER_NAME, "GameObjectPoolGroup::CreatePool->Template is Null");
                return null;
            }

            if (poolDic.TryGetValue(uniqueName, out GameObjectPool goPool))
            {
                LogUtil.LogWarning(GameObjectPoolConst.LOGGER_NAME, "GameObjectPoolGroup::CreatePool->The pool has been created.uniqueName = " + uniqueName);
            }
            else
            {
                goPool = new GameObjectPool(name,GetTransform(), uniqueName, templateType, template);
                poolDic.Add(uniqueName, goPool);
            }

            return goPool;
        }
        /// <summary>
        /// 删除指定的缓存池
        /// </summary>
        /// <param name="uniqueName">资源唯一标签，一般使用资源路径</param>
        public void DeletePool(string uniqueName)
        {
            LogUtil.LogInfo(GameObjectPoolConst.LOGGER_NAME, $"GameObjectPoolGroup::DeletePool->Try to delete pool.uniqueName ={uniqueName}");
            
            GameObjectPool gObjPool = GetPool(uniqueName);

            if(gObjPool!=null)
            {
                gObjPool.Dispose();
                poolDic.Remove(uniqueName);
            }
            else
            {
                LogUtil.LogWarning(GameObjectPoolConst.LOGGER_NAME, "The pool is not found.name = " + uniqueName);
            }
        }

        internal void Dispose()
        {
            foreach (var kvp in poolDic)
            {
                kvp.Value.Dispose();
            }
            poolDic.Clear();

            if(groupTransform!=null)
            {
                Object.Destroy(groupTransform.gameObject);
            }
        }
    }
}
