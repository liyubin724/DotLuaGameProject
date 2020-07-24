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
        internal string GroupName { get; private set; } = string.Empty;
        internal Transform GroupTransform { get; private set; } = null;

        private Dictionary<string, GameObjectPool> poolDic = new Dictionary<string, GameObjectPool>();

        internal GameObjectPoolGroup(string gName, Transform parentTran)
        {
            GroupName = gName;

            GroupTransform = new GameObject(string.Format(GameObjectPoolConst.GROUP_NAME_FORMAT,GroupName)).transform;
            GroupTransform.SetParent(parentTran, false);
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
            }

            return null;
        }

        /// <summary>
        /// 使用给定的GameObject创建缓存池
        /// </summary>
        /// <param name="uniqueName">资源唯一标签，一般使用资源路径</param>
        /// <param name="template">模板GameObject</param>
        /// <returns></returns>
        public GameObjectPool CreatePool(string uniqueName,GameObject template, PoolTemplateType templateType = PoolTemplateType.Prefab)
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
                goPool = new GameObjectPool(this, uniqueName, template, templateType);
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
                gObjPool.DestroyPool();
                poolDic.Remove(uniqueName);
            }
        }

        internal void CullGroup(float deltaTime)
        {
            foreach(var kvp in poolDic)
            {
                kvp.Value.CullPool(deltaTime);
            }
        }

        internal void DestroyGroup()
        {
            foreach(var kvp in poolDic)
            {
                kvp.Value.DestroyPool();
            }
            poolDic.Clear();

            Object.Destroy(GroupTransform.gameObject);
        }
    }
}
