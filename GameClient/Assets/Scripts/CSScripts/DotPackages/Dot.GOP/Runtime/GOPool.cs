using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOP
{
    public class GOPool 
    {
        public const string NAME = "GOPool";

        private Transform rootTransform = null;
        private Dictionary<string, GOPoolCategory> categoryDic = new Dictionary<string, GOPoolCategory>();

        public GOPool()
        {
            GameObject go = new GameObject(NAME);
            rootTransform = go.transform;
            UnityObject.DontDestroyOnLoad(go);
        }

        /// <summary>
        /// 判断是否存在指定的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasCategory(string name)=> categoryDic.ContainsKey(name);

        /// <summary>
        ///获取指定的分组，如果不存在可以指定isCreateIfNot为true进行创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoCreateIfNot"></param>
        /// <returns></returns>
        public GOPoolCategory GetCategory(string name)
        {
            if (categoryDic.TryGetValue(name, out GOPoolCategory category))
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
        public GOPoolCategory CreateCategory(string name)
        {
            if (!categoryDic.TryGetValue(name, out GOPoolCategory category))
            {
                category = new GOPoolCategory(name, rootTransform);
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
            if (categoryDic.TryGetValue(name, out GOPoolCategory group))
            {
                categoryDic.Remove(name);
                group.Dispose();
            }
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        public void Dispose()
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.Dispose();
            }
            categoryDic.Clear();

            UnityObject.Destroy(rootTransform);
        }
    }
}
