using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public class PoolManager
    {
        private static readonly string POOL_ROOT_NAME = "PoolMgr";

        private static PoolManager manager = null;
        public static PoolManager GetInstance() => manager;

        public static PoolManager CreateMgr()
        {
            if (manager == null)
            {
                manager = new PoolManager();
                manager.DoInitialize();
            }
            return manager;
        }

        public static void DestroyMgr()
        {
            if (manager != null)
            {
                manager.DoDestroy();
            }
            manager = null;
        }

        private Transform mgrTransform = null;
        private Dictionary<string, PoolCategory> categoryDic = new Dictionary<string, PoolCategory>();

        private PoolManager()
        {
        }

        private void DoInitialize()
        {
            mgrTransform = PersistentUObjectHelper.CreateTransform(POOL_ROOT_NAME);
        }

        private void DoDestroy()
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoDestroy();
            }
            categoryDic.Clear();

            UnityObject.Destroy(mgrTransform.gameObject);
            mgrTransform = null;
        }

        /// <summary>
        /// 判断是否存在指定的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasCategory(string name) => categoryDic.ContainsKey(name);

        /// <summary>
        ///获取指定的分组，如果不存在可以指定isCreateIfNot为true进行创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoCreateIfNot"></param>
        /// <returns></returns>
        public PoolCategory GetCategory(string name)
        {
            if (categoryDic.TryGetValue(name, out PoolCategory category))
            {
                return category;
            }
            return null;
        }

        /// <summary>
        /// 创建指定名称的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PoolCategory CreateCategory(string name)
        {
            if (!categoryDic.TryGetValue(name, out PoolCategory category))
            {
                category = new PoolCategory(name, mgrTransform);
                categoryDic.Add(name, category);
            }
            return category;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCategory(string name)
        {
            if (categoryDic.TryGetValue(name, out PoolCategory group))
            {
                categoryDic.Remove(name);
                group.DoDestroy();
            }
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }
    }
}
