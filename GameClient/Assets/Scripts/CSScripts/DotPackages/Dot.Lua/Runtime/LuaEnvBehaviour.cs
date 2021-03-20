﻿using UnityEngine;

namespace DotEngine.Lua
{
    public class LuaEnvBehaviour : MonoBehaviour
    {
        private void Update()
        {
            LuaEnvManager.GetInstance().DoUpdate();
        }

        private void LateUpdate()
        {
            LuaEnvManager.GetInstance().DoLateUpdate();
        }

        private void OnDestroy()
        {
            LuaEnvManager.GetInstance().DoDestroy();
        }
    }
}
