using UnityEngine;

namespace DotEngine.Lua
{
    public class LuaEnvBehaviour : MonoBehaviour
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
