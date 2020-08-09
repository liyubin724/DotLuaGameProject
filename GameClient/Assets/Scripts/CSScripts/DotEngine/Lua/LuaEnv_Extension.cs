#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
#else
using LuaAPI = XLua.LuaDLL.Lua;
#endif


namespace XLua
{
    public static class LuaEnv_Extension
    {
        public static int GetTotalMemory(this LuaEnv env)
        {
            int totalMemory = LuaAPI.lua_gc(env.L, LuaGCOptions.LUA_GCCOUNT, 0);
            return totalMemory;
        }
    }
}
