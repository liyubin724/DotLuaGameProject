using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.GOP
{
    public class GOPContainer 
    {
        public const string NAME = "GOPool";

        private Transform containerTransform = null;
        private Dictionary<string, GOPCategory> categoryDic = new Dictionary<string, GOPCategory>();

        public GOPContainer()
        {
            Transform root = GOPUtil.Root;
#if DEBUG
            GameObject go = new GameObject(NAME);
            containerTransform = go.transform;

            containerTransform.SetParent(root, false);
#else
            containerTransform = root;
#endif
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
        public GOPCategory GetCategory(string name)
        {
            if (categoryDic.TryGetValue(name, out GOPCategory category))
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
        public GOPCategory CreateCategory(string name)
        {
            if (!categoryDic.TryGetValue(name, out GOPCategory category))
            {
                category = new GOPCategory(name, containerTransform);
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
            if (categoryDic.TryGetValue(name, out GOPCategory group))
            {
                categoryDic.Remove(name);
                group.Destroy();
            }
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.DoUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        public void Destroy()
        {
            foreach (var kvp in categoryDic)
            {
                kvp.Value.Destroy();
            }
            categoryDic.Clear();

#if DEBUG
            GameObject.Destroy(containerTransform.gameObject);
#endif
        }
    }
}
