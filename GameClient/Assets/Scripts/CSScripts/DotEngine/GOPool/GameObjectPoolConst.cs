using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOPool
{
    public static class GameObjectPoolConst
    {
        internal static readonly string LOGGER_NAME = "GOPool";

        internal static readonly string MANAGER_NAME = "GOService";
        internal static readonly string GROUP_NAME_FORMAT = "Group{0}";

        public static Func<string, UnityObject, UnityObject> InstantiateAsset;
    }
}
