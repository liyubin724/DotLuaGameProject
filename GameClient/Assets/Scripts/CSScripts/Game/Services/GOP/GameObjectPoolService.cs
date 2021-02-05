using DotEngine.Framework.Services;
using DotEngine.GOP;
using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Game.Services
{
    public class GameObjectPoolService : Service, IGOPoolService
    {
        public readonly static string NAME = "GOPService";

        private GOPool goPool = null;
        public GameObjectPoolService():base(NAME)
        {
        }

        public override void DoRegistered()
        {
            goPool = new GOPool();
        }

        public override void DoUnregistered()
        {
            goPool.Dispose();
            goPool = null;
        }

        public void InitPool(
            Func<string, UnityObject, UnityObject> instantiateAssetFunc,
            Action<string, string> infoAction,
            Action<string, string> warningAction,
            Action<string, string> errorAction)
        {
            GOPoolUtil.InstantiateAsset = instantiateAssetFunc;
            GOPoolUtil.LogInfoAction = infoAction;
            GOPoolUtil.LogWarningAction = warningAction;
            GOPoolUtil.LogErrorAction = errorAction;
        }

        #region Category
        public bool HasCategory(string categoryName)
        {
            return goPool.HasCategory(categoryName);
        }

        public GOPoolCategory GetCategory(string categoryName)
        {
            return goPool.GetCategory(categoryName);
        }

        public GOPoolCategory CreateCategory(string categoryName)
        {
            return goPool.CreateCategory(categoryName);
        }

        public void DeleteCategory(string categoryName)
        {
            goPool.DeleteGroup(categoryName);
        }
        #endregion

        #region ItemGroup
        public bool HasGroup(string categoryName, string groupName)
        {
            GOPoolCategory category = GetCategory(categoryName);
            if(category == null)
            {
                return false;
            }
            return category.HasGroup(groupName);
        }

        public GOPoolItemGroup GetGroup(string categoryName, string groupName)
        {
            GOPoolCategory category = GetCategory(categoryName);
            if (category == null)
            {
                return null;
            }
            return category.GetGroup(groupName);
        }

        public GOPoolItemGroup CreateGroup(string categoryName, string groupName, ItemTemplateType itemType, GameObject itemTemplate)
        {
            GOPoolCategory category = GetCategory(categoryName);
            if (category == null)
            {
                return null;
            }
            return category.CreateGroup(groupName,itemType,itemTemplate);
        }

        public void DeleteGroup(string categoryName, string groupName)
        {
            GOPoolCategory category = GetCategory(categoryName);
            if (category == null)
            {
                return;
            }
            category.DeleteGroup(groupName);
        }

        #endregion

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            goPool.DoUpdate(deltaTime, unscaleDeltaTime);
        }
    }
}
