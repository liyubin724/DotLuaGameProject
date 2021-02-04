using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;
using DotEngine.GOP;
using UnityEngine;

namespace Game.Services
{
    public interface IGameObjectPoolService : IService, IUpdate
    {
        bool HasGroup(string name);
        GOPoolGroup GetGroup(string name);
        GOPoolGroup CreateGroup(string name);
        void DeleteGroup(string name);

        bool HasPool(string groupName, string poolName);
        GOPool GetPool(string groupName,string poolName);
        GOPool CreatePool(string groupName, string poolName, PoolTemplateType templateType, GameObject template);
        void DeletePool(string groupName, string poolName);

    }
}
