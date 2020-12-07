﻿using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOP
{
    public static class GameObjectPoolConst
    {
        internal static readonly string LOGGER_NAME = "GOPool";

        public static Func<string, UnityObject, UnityObject> InstantiateAsset;
    }
}