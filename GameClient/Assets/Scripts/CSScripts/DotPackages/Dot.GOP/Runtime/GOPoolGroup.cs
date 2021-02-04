using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.GOP
{
    /// <summary>
    /// 主要用于GameObjectPool的分组，可以根据使用场景进行添加和删除分组，不同的分组中可以有相同GameObject的缓存池
    /// </summary>
    public class GOPoolGroup
    {
        private string name;
        private Transform groupTransform = null;

        private Dictionary<string, GOPool> poolDic = new Dictionary<string, GOPool>();

        internal GOPoolGroup(string gName, Transform parentTran)
        {
            name = gName;
#if UNITY_EDITOR
            groupTransform = new GameObject(name).transform;
            groupTransform.SetParent(parentTran, false);
#else
            groupTransform = parentTran;
#endif
        }

        public bool HasPool(string name)
        {
            return poolDic.ContainsKey(name);
        }

        /// <summary>
        /// 缓存池中将默认以资源的路径为唯一标识，通过资源唯一标识可以获致到对应的缓存池
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GOPool GetPool(string name)
        {
            if(poolDic.TryGetValue(name,out GOPool goPool))
            {
                return goPool;
            }else
            {
                GOPoolUtil.LogWarning("The pool is not found.name = " + name);
            }

            return null;
        }

        /// <summary>
        /// 使用给定的GameObject创建缓存池
        /// </summary>
        /// <param name="poolName">资源唯一标签，一般使用资源路径</param>
        /// <param name="template">模板GameObject</param>
        /// <returns></returns>
        public GOPool CreatePool(string poolName, PoolTemplateType templateType, GameObject template)
        {
            if(template == null)
            {
                GOPoolUtil.LogError("Template is Null");
                return null;
            }

            if (!poolDic.TryGetValue(poolName, out GOPool goPool))
            {
                goPool = new GOPool(name, groupTransform, poolName, templateType, template);
                poolDic.Add(poolName, goPool);
            }
            else
            {
                GOPoolUtil.LogWarning("The pool has been created.uniqueName = " + poolName);
            }

            return goPool;
        }
        /// <summary>
        /// 删除指定的缓存池
        /// </summary>
        /// <param name="poolName">资源唯一标签，一般使用资源路径</param>
        public void DeletePool(string poolName)
        {
            GOPool gObjPool = GetPool(poolName);
            if (gObjPool != null)
            {
                gObjPool.Dispose();
                poolDic.Remove(poolName);
            }
            else
            {
                GOPoolUtil.LogWarning("The pool is not found.name = " + poolName);
            }
        }

        internal void Dispose()
        {
            foreach (var kvp in poolDic)
            {
                kvp.Value.Dispose();
            }
            poolDic.Clear();

#if UNITY_EDITOR
            Object.Destroy(groupTransform.gameObject);
#endif
        }
    }
}
