using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;
using DotEngine.GOP;
using UnityEngine;

namespace Game.Services
{
    public interface IGameObjectPoolService : IService, IUpdate
    {
        bool HasGroup(string name);
        GameObjectPoolGroup GetGroup(string name);
        GameObjectPoolGroup CreateGroup(string name);
        void DeleteGroup(string name);

        bool HasPool(string groupName, string poolName);
        GameObjectPool GetPool(string groupName,string poolName);
        GameObjectPool CreatePool(string groupName, string poolName, PoolTemplateType templateType, GameObject template);
        void DeletePool(string groupName, string poolName);

    }
}
