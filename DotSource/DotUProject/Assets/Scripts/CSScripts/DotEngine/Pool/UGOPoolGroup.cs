using DotEngine.Log;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    /// <summary>
    /// 主要用于分组，可以根据使用场景进行添加和删除分组，不同的分组中可以有相同GameObject的缓存池
    /// </summary>
    public class UGOPoolGroup
    {
        public string Name { get; private set; }

        private Transform groupTransform = null;
        private Dictionary<string, UGOPool> poolDic = new Dictionary<string, UGOPool>();

        internal UGOPoolGroup(string name, Transform parentTran)
        {
            Name = name;
            if (UGOPoolUtill.IsDebug)
            {
                groupTransform = new GameObject(Name).transform;
                groupTransform.SetParent(parentTran, false);
            }
            else
            {
                groupTransform = parentTran;
            }
        }

        public bool HasPool(string assetPath) => poolDic.ContainsKey(assetPath);

        /// <summary>
        /// 缓存池中将默认以资源的路径为唯一标识，通过资源唯一标识可以获致到对应的缓存池
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UGOPool GetPool(string assetPath)
        {
            if (poolDic.TryGetValue(assetPath, out UGOPool pool))
            {
                return pool;
            }

            LogUtil.Warning(UGOPoolUtill.LOG_TAG, "The pool is not found.name = " + assetPath);
            return null;
        }

        /// <summary>
        /// 使用给定的GameObject创建缓存池
        /// </summary>
        /// <param name="assetPath">资源唯一标签，一般使用资源路径</param>
        /// <param name="template">模板GameObject</param>
        /// <returns></returns>
        public UGOPool CreatePool(string assetPath, UGOTemplateType itemType, GameObject template)
        {
            if (template == null)
            {
                LogUtil.Error(UGOPoolUtill.LOG_TAG, "Template is Null");
                return null;
            }
            if (poolDic.ContainsKey(assetPath))
            {
                LogUtil.Error(UGOPoolUtill.LOG_TAG, "The pool has been created.uniqueName = " + assetPath);
                return null;
            }

            UGOPool pool = new UGOPool(groupTransform, assetPath, itemType, template);
            poolDic.Add(assetPath, pool);
            return pool;
        }

        /// <summary>
        /// 删除指定的缓存池
        /// </summary>
        /// <param name="assetPath">资源唯一标签，一般使用资源路径</param>
        public void DestroyPool(string assetPath)
        {
            if(poolDic.TryGetValue(assetPath,out var pool))
            {
                poolDic.Remove(assetPath);
                pool.Destroy();
            }
        }

        internal void DoUpdate(float deltaTime)
        {
            foreach (var kvp in poolDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }

        internal void Destroy()
        {
            foreach (var kvp in poolDic)
            {
                kvp.Value.Destroy();
            }
            poolDic.Clear();

            if (UGOPoolUtill.IsDebug)
            {
                UnityObject.Destroy(groupTransform.gameObject);
            }
        }
    }
}
