#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class DotEngineLogLogUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(DotEngine.Log.LogUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 8, 2, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLogger", _m_SetLogger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DisposeLogger", _m_DisposeLogger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogDebug", _m_LogDebug_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogInfo", _m_LogInfo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogWarning", _m_LogWarning_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogError", _m_LogError_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LogFatal", _m_LogFatal_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LimitLevel", _g_get_LimitLevel);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsEnable", _g_get_IsEnable);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LimitLevel", _s_set_LimitLevel);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "IsEnable", _s_set_IsEnable);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "DotEngine.Log.LogUtil does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLogger_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    DotEngine.Log.ILogger _logger = (DotEngine.Log.ILogger)translator.GetObject(L, 1, typeof(DotEngine.Log.ILogger));
                    
                    DotEngine.Log.LogUtil.SetLogger( _logger );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DisposeLogger_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    DotEngine.Log.LogUtil.DisposeLogger(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogDebug_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    DotEngine.Log.LogUtil.LogDebug( _tag, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogInfo_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    DotEngine.Log.LogUtil.LogInfo( _tag, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogWarning_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    DotEngine.Log.LogUtil.LogWarning( _tag, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogError_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    DotEngine.Log.LogUtil.LogError( _tag, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogFatal_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    DotEngine.Log.LogUtil.LogFatal( _tag, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LimitLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, DotEngine.Log.LogUtil.LimitLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsEnable(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, DotEngine.Log.LogUtil.IsEnable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LimitLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			DotEngine.Log.LogLevelType gen_value;translator.Get(L, 1, out gen_value);
				DotEngine.Log.LogUtil.LimitLevel = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsEnable(RealStatePtr L)
        {
		    try {
                
			    DotEngine.Log.LogUtil.IsEnable = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
