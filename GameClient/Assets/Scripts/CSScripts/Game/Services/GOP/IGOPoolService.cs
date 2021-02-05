using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;
using DotEngine.GOP;
using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Game.Services
{
    public interface IGOPoolService : IService, IUpdate
    {
        void InitPool(
            Func<string, UnityObject, UnityObject> instantiateAssetFunc,
            Action<string, string> infoAction, 
            Action<string, string> warningAction, 
            Action<string, string> errorAction);

        bool HasCategory(string categoryName);
        GOPoolCategory GetCategory(string categoryName);
        GOPoolCategory CreateCategory(string categoryName);
        void DeleteCategory(string categoryName);

        bool HasGroup(string categoryName, string groupName);
        GOPoolItemGroup GetGroup(string categoryName, string groupName);
        GOPoolItemGroup CreateGroup(string categoryName, string groupName, ItemTemplateType itemType, GameObject itemTemplate);
        void DeleteGroup(string categoryName, string groupName);

    }
}
