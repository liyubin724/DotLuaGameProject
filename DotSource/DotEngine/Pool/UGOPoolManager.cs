using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public class UGOPoolManager : Singleton<UGOPoolManager>
    {
        private static readonly string ROOT_NAME = "PoolMgr";

        private Transform mgrTransform = null;
        private Dictionary<string, UGOPoolGroup> groupDic = new Dictionary<string, UGOPoolGroup>();

        public UGOPoolManager()
        {
        }

        protected override void OnInit()
        {
            mgrTransform = PersistentUObjectHelper.CreateTransform(ROOT_NAME);
        }

        protected override void OnDestroy()
        {
            foreach (var kvp in groupDic)
            {
                kvp.Value.Destroy();
            }
            groupDic.Clear();

            UnityObject.Destroy(mgrTransform.gameObject);
            mgrTransform = null;

            base.OnDestroy();
        }

        /// <summary>
        /// 判断是否存在指定的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasGroup(string name) => groupDic.ContainsKey(name);

        /// <summary>
        ///获取指定的分组，如果不存在可以指定isCreateIfNot为true进行创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoCreateIfNot"></param>
        /// <returns></returns>
        public UGOPoolGroup GetGroup(string name)
        {
            if (groupDic.TryGetValue(name, out UGOPoolGroup group))
            {
                return group;
            }
            return null;
        }

        /// <summary>
        /// 创建指定名称的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UGOPoolGroup CreateGroup(string name)
        {
            if (!groupDic.TryGetValue(name, out UGOPoolGroup group))
            {
                group = new UGOPoolGroup(name, mgrTransform);
                groupDic.Add(name, group);
            }
            return group;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DestroyGroup(string name)
        {
            if (groupDic.TryGetValue(name, out UGOPoolGroup group))
            {
                groupDic.Remove(name);
                group.Destroy();
            }
        }

        public UGOPool GetPool(string groupName,string assetPath)
        {
            if (groupDic.TryGetValue(groupName, out UGOPoolGroup group))
            {
                return group.GetPool(assetPath);
            }
            return null;
        }

        public UGOPool CreatePool(string groupName, string assetPath, UGOTemplateType itemType, GameObject template)
        {
            UGOPoolGroup group = CreateGroup(groupName);
            return group.CreatePool(assetPath, itemType, template);
        }

        public void DestroyPool(string groupName,string assetPath)
        {
            UGOPoolGroup group = CreateGroup(groupName);
            group.DestroyPool(assetPath);
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (var kvp in groupDic)
            {
                kvp.Value.DoUpdate(deltaTime);
            }
        }
    }
}
