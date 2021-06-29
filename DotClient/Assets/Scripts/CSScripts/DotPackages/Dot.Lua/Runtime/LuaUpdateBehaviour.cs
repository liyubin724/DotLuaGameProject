using UnityEngine;

namespace DotEngine.Lua
{
    public class LuaUpdateBehaviour : MonoBehaviour
    {

        private void Start()
        {
            LuaEnvManager.GetInstance().DoStart();
        }

        private void Update()
        {
            LuaEnvManager.GetInstance().DoUpdate();
        }

        private void LateUpdate()
        {
            LuaEnvManager.GetInstance().DoLateUpdate();
        }
    }
}
