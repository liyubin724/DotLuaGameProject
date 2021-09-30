using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UPool
{
    public class GameObjectPool
    {
        public const string NAME = "GameObjectPool";

        private Transform containerTransform = null;
        private Dictionary<string, GameObjectCategory> categoryDic = new Dictionary<string, GameObjectCategory>();

        public GameObjectPool()
        {
            Transform root = PoolUtill.Root;
            if (PoolUtill.IsInDebug)
            {
                GameObject go = new GameObject(NAME);
                containerTransform = go.transform;
                containerTransform.SetParent(root, false);
            }
            else
            {
                containerTransform = root;
            }
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
        public GameObjectCategory GetCategory(string name)
        {
            if (categoryDic.TryGetValue(name, out GameObjectCategory category))
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
        public GameObjectCategory CreateCategory(string name)
        {
            if (!categoryDic.TryGetValue(name, out GameObjectCategory category))
            {
                category = new GameObjectCategory(name, containerTransform);
                categoryDic.Add(name, category);
            }
            return category;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteGroup(string name)
        {
            if (categoryDic.TryGetValue(name, out GameObjectCategory group))
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

        public void DoDestroy()
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoDestroy();
            }
            categoryDic.Clear();

            Object.Destroy(containerTransform.gameObject);
        }
    }
}
