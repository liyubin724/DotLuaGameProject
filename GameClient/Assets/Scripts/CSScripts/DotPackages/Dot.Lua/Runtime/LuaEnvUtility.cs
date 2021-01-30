using RealStatePtr = System.IntPtr;

namespace XLua
{
    public static class LuaEnvUtility
    {
        public static bool IsValid(this LuaEnv luaEnv)
        {
            return luaEnv.L != RealStatePtr.Zero;
        }
    }
}
