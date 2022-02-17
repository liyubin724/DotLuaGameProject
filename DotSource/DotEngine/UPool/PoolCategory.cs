using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UPool
{
    /// <summary>
    /// 主要用于分组，可以根据使用场景进行添加和删除分组，不同的分组中可以有相同GameObject的缓存池
    /// </summary>
    public class PoolCategory
    {
        public string Name { get; private set; }

        private Transform categoryTransform = null;
        private Dictionary<string, PoolGroup> itemGroupDic = new Dictionary<string, PoolGroup>();

        internal PoolCategory(string name, Transform parentTran)
        {
            Name = name;
            if (PoolUtill.IsDebug)
            {
                categoryTransform = new GameObject(Name).transform;
                categoryTransform.SetParent(parentTran, false);
            }
            else
            {
                categoryTransform = parentTran;
            }
        }

        public bool HasGroup(string name) => itemGroupDic.ContainsKey(name);

        /// <summary>
        /// 缓存池中将默认以资源的路径为唯一标识，通过资源唯一标识可以获致到对应的缓存池
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PoolGroup GetGroup(string name)
        {
            if (itemGroupDic.TryGetValue(name, out PoolGroup itemGroup))
            {
                return itemGroup;
            }
            PoolUtill.Warning("The pool is not found.name = " + name);
            return null;
        }

        /// <summary>
        /// 使用给定的GameObject创建缓存池
        /// </summary>
        /// <param name="itemName">资源唯一标签，一般使用资源路径</param>
        /// <param name="itemTemplate">模板GameObject</param>
        /// <returns></returns>
        public PoolGroup CreateGroup(string itemName, TemplateType itemType, GameObject itemTemplate)
        {
            if (itemTemplate == null)
            {
                PoolUtill.Error("Template is Null");
                return null;
            }

            if (!itemGroupDic.TryGetValue(itemName, out PoolGroup itemGroup))
            {
                itemGroup = new PoolGroup(Name, categoryTransform, itemName, itemType, itemTemplate);
                itemGroupDic.Add(itemName, itemGroup);
                return itemGroup;
            }
            PoolUtill.Warning("The pool has been created.uniqueName = " + itemName);
            return null;
        }

        /// <summary>
        /// 删除指定的缓存池
        /// </summary>
        /// <param name="name">资源唯一标签，一般使用资源路径</param>
        public void DeleteGroup(string name)
        {
            PoolGroup itemGroup = GetGroup(name);
            if (itemGroup != null)
            {
                itemGroupDic.Remove(name);
                itemGroup.DoDestroy();
            }
            else
            {
                PoolUtill.Warning("The pool is not found.name = " + name);
            }
        }

        internal void DoUpdate(float deltaTime)
        {
            foreach (var kvp in itemGroupDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }

        internal void DoDestroy()
        {
            foreach (var kvp in itemGroupDic)
            {
                kvp.Value.DoDestroy();
            }
            itemGroupDic.Clear();
            if(PoolUtill.IsDebug)
            {
                Object.Destroy(categoryTransform.gameObject);
            }
        }
    }
}
