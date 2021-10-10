using DotEngine.Core;
using DotEngine.Core.Update;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UPool
{
    public class PoolManager : IUpdate
    {
        private static readonly string POOL_ROOT_NAME = "PoolMgr";

        public static readonly string GLOBAL_CATEGORY_NAME = "Global Category";
        public static readonly string GLOBAL_GOBJ_ROOT_GROUP_NAME = "Global Root Group";

        private static PoolManager manager = null;
        public static PoolManager InitMgr()
        {
            if (manager == null)
            {
                manager = new PoolManager();
                manager.DoInitialize();
            }
            return manager;
        }

        public static PoolManager GetInstance() => manager;

        public static void DisposeMgr()
        {
            if (manager != null)
            {
                manager.DoDestroy();
            }
            manager = null;
        }

        private Transform containerTransform = null;
        private Dictionary<string, PoolCategory> categoryDic = new Dictionary<string, PoolCategory>();

        private PoolManager()
        {
        }

        private void DoInitialize()
        {
            containerTransform = PersistentUObjectHelper.CreateTransform(POOL_ROOT_NAME);
            UpdateManager.GetInstance().AddUpdater(this);

            PoolCategory globalCategory = CreateCategory(GLOBAL_CATEGORY_NAME);
            GameObject rootGObject = new GameObject("Root GObject");
            PoolGroup rootGObjGroup = globalCategory.CreateGroup(GLOBAL_GOBJ_ROOT_GROUP_NAME, TemplateType.RuntimeInstance, rootGObject);
            rootGObjGroup.SetPreload(10, 1, null);
        }

        private void DoDestroy()
        {
            UpdateManager.GetInstance().RemoveUpdater(this);
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoDestroy();
            }
            categoryDic.Clear();

            Object.Destroy(containerTransform.gameObject);
            containerTransform = null;
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
                category = new PoolCategory(name, containerTransform);
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

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoUpdate(deltaTime, unscaleDeltaTime);
            }
        }
    }
}
