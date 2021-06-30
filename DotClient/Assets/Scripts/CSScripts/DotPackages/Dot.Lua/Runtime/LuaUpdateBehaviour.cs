using UnityEngine;

namespace DotEngine.Lua
{
    public class LuaUpdateBehaviour : MonoBehaviour
    {
        void Update()
        {
            LuaEnvManager.GetInstance().DoUpdate(Time.deltaTime,Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            LuaEnvManager.GetInstance().DoLateUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void FixedUpdate()
        {
            LuaEnvManager.GetInstance().DoFixedUpdate(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }
    }
}
