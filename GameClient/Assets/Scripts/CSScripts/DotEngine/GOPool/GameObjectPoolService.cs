using DotEngine.Services;
using DotEngine.Timer;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOPool
{
    public class GameObjectPoolService : Service
    {
        public const string NAME = "GOPoolService";

        private Transform rootTransform = null;
        private Dictionary<string, GameObjectPoolGroup> groupDic = new Dictionary<string, GameObjectPoolGroup>();

        private float cullTimeInterval = 60f;
        private TimerHandler cullTimerTask = null;

        public GameObjectPoolService(Func<string, UnityObject, UnityObject> instantiateAsset) : base(NAME)
        {
            GameObjectPoolConst.InstantiateAsset = instantiateAsset;
        }

        public override void DoRegister()
        {
            rootTransform = DontDestroyHandler.CreateTransform(GameObjectPoolConst.MANAGER_NAME);

            TimerService timerService = Facade.GetInstance().GetService<TimerService>(TimerService.NAME);
            cullTimerTask = timerService.AddIntervalTimer(cullTimeInterval, OnCullTimerUpdate);
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
        public GameObjectPoolGroup GetGroup(string name,bool autoCreateIfNot = false)
        {
            if (groupDic.TryGetValue(name, out GameObjectPoolGroup pool))
            {
                return pool;
            }

            if(autoCreateIfNot)
            {
                return CreateGroup(name);
            }
            return null;
        }

        /// <summary>
        /// 创建指定名称的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObjectPoolGroup CreateGroup(string name)
        {
            if (!groupDic.TryGetValue(name, out GameObjectPoolGroup pool))
            {
                pool = new GameObjectPoolGroup(name, rootTransform);
                groupDic.Add(name, pool);
            }
            return pool;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteGroup(string name)
        {
            if (groupDic.TryGetValue(name, out GameObjectPoolGroup spawn))
            {
                groupDic.Remove(name);
                spawn.DestroyGroup();
            }
        }

        private void OnCullTimerUpdate(System.Object obj)
        {
            foreach(var kvp in groupDic)
            {
                kvp.Value.CullGroup(cullTimeInterval);
            }
        }

        public override void DoRemove()
        {
            GameObjectPoolConst.InstantiateAsset = null;

            foreach (var kvp in groupDic)
            {
                kvp.Value.DestroyGroup();
            }
            groupDic.Clear();

            if (cullTimerTask != null)
            {
                TimerService timerService = Facade.GetInstance().GetService<TimerService>(TimerService.NAME);
                timerService.RemoveTimer(cullTimerTask);
                cullTimerTask = null;
            }
            cullTimerTask = null;
            groupDic = null;

            UnityObject.Destroy(rootTransform);
        }
    }
}
