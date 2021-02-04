using DotEngine.Framework.Services;
using DotEngine.GOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Game.Services
{
    public class GameObjectPoolService : Service, IGameObjectPoolService
    {
        public readonly static string NAME = "GOP Service";

        private Transform rootTransform = null;
        private Dictionary<string, GameObjectPoolGroup> groupDic = new Dictionary<string, GameObjectPoolGroup>();

        public GameObjectPoolService():base(NAME)
        {
        }

        public override void DoRegistered()
        {
            GameObject go = new GameObject(NAME);
            go.hideFlags = HideFlags.HideAndDontSave;
            rootTransform = go.transform;

            UnityObject.DontDestroyOnLoad(go);
        }

        public override void DoUnregistered()
        {
            foreach (var kvp in groupDic)
            {
                kvp.Value.Dispose();
            }
            groupDic.Clear();

            UnityObject.Destroy(rootTransform);
        }

        #region Group
        public bool HasGroup(string name)
        {
            return groupDic.ContainsKey(name);
        }

        public GameObjectPoolGroup CreateGroup(string name)
        {
            if(!groupDic.TryGetValue(name,out var group))
            {
                group = new GameObjectPoolGroup(name, rootTransform);
                groupDic.Add(name, group);
            }
            return group;
        }

        public GameObjectPoolGroup GetGroup(string name)
        {
            if (groupDic.TryGetValue(name, out var group))
            {
                return group;
            }
            return null;
        }

        public void DeleteGroup(string name)
        {
            if (groupDic.TryGetValue(name, out GameObjectPoolGroup group))
            {
                groupDic.Remove(name);
                group.Dispose();
            }
        }
        #endregion

        #region Pool

        public bool HasPool(string groupName,string poolName)
        {
            GameObjectPoolGroup group = GetGroup(groupName);
            if(group==null)
            {
                return false;
            }
            return group.HasPool(poolName);
        }

        public GameObjectPool GetPool(string groupName,string poolName)
        {
            GameObjectPoolGroup group = GetGroup(groupName);
            if (group == null)
            {
                return null;
            }
            return group.GetPool(poolName);
        }

        public GameObjectPool CreatePool(string groupName,string poolName, PoolTemplateType templateType, GameObject template)
        {
            GameObjectPoolGroup group = GetGroup(groupName);
            if(group == null)
            {
                group = CreateGroup(groupName);
            }
            return group.CreatePool(poolName, templateType, template);
        }

        public void DeletePool(string groupName,string poolName)
        {
            GameObjectPoolGroup group = GetGroup(groupName);
            if (group != null)
            {
                group.DeletePool(poolName);
            }
        }
        #endregion

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            
        }

        

        
    }
}
