using DotEngine.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameStartBehaviour : MonoBehaviour
    {
        void Awake()
        {
            LuaEnvManager.GetInstance().Startup();
        }

        private void OnDestroy()
        {
            LuaEnvManager.GetInstance().Shuntdown();
            LuaEnvManager.GetInstance().Dispose();
        }
    }

}

