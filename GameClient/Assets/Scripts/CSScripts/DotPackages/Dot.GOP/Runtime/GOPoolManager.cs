using DotEngine.Services;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOP
{
    public class GOPoolManager 
    {
        public const string NAME = "GOPool";

        private Transform rootTransform = null;
        private Dictionary<string, GOPoolGroup> groupDic = new Dictionary<string, GOPoolGroup>();

        public GOPoolManager()
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
        public bool HasGroup(string name)=> groupDic.ContainsKey(name);

        /// <summary>
        ///获取指定的分组，如果不存在可以指定isCreateIfNot为true进行创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoCreateIfNot"></param>
        /// <returns></returns>
        public GOPoolGroup GetGroup(string name)
        {
            if (groupDic.TryGetValue(name, out GOPoolGroup pool))
            {
                return pool;
            }
            return null;
        }

        /// <summary>
        /// 创建指定名称的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GOPoolGroup CreateGroup(string name)
        {
            if (!groupDic.TryGetValue(name, out GOPoolGroup group))
            {
                group = new GOPoolGroup(name, rootTransform);
                groupDic.Add(name, group);
            }
            return group;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteGroup(string name)
        {
            if (groupDic.TryGetValue(name, out GOPoolGroup group))
            {
                groupDic.Remove(name);
                group.Dispose();
            }
        }

        public override void DoRemove()
        {
            GOPoolUtil.InstantiateAsset = null;

            foreach (var kvp in groupDic)
            {
                kvp.Value.Dispose();
            }
            groupDic.Clear();

            UnityObject.Destroy(rootTransform);
        }
    }
}
