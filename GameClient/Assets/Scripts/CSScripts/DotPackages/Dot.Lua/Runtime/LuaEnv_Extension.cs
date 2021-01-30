#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
#else
using LuaAPI = XLua.LuaDLL.Lua;
#endif


namespace XLua
{
    public static class LuaEnv_Extension
    {
        public static float GetTotalMemory(this LuaEnv env)
        {
            int memoryInK = LuaAPI.lua_gc(env.L, LuaGCOptions.LUA_GCCOUNT, 0);
            int memoryInB = LuaAPI.lua_gc(env.L, LuaGCOptions.LUA_GCCOUNTB, 0);
            
            return memoryInK + (memoryInB / 1024.0f);
        }
    }
}
