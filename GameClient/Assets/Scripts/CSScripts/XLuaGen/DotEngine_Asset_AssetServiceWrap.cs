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
    public class DotEngineAssetAssetServiceWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(DotEngine.Asset.AssetService);
			Utils.BeginObjectRegister(type, L, translator, 0, 14, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DoUpdate", _m_DoUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DoRemove", _m_DoRemove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitDatabaseLoader", _m_InitDatabaseLoader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitBundleLoader", _m_InitBundleLoader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeMaxLoadingCount", _m_ChangeMaxLoadingCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAssetAsync", _m_LoadAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InstanceAssetAsync", _m_InstanceAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadBatchAssetAsync", _m_LoadBatchAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadBatchAssetAsyncByLabel", _m_LoadBatchAssetAsyncByLabel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InstanceBatchAssetAsync", _m_InstanceBatchAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InstanceBatchAssetAsyncByLabel", _m_InstanceBatchAssetAsyncByLabel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAssetAsync", _m_UnloadAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InstantiateAsset", _m_InstantiateAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadUnusedAsset", _m_UnloadUnusedAsset);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NAME", DotEngine.Asset.AssetService.NAME);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					DotEngine.Asset.AssetService gen_ret = new DotEngine.Asset.AssetService();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _deltaTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.DoUpdate( _deltaTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoRemove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DoRemove(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitDatabaseLoader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<bool> _initCallback = translator.GetDelegate<System.Action<bool>>(L, 2);
                    
                    gen_to_be_invoked.InitDatabaseLoader( _initCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitBundleLoader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<bool> _initCallback = translator.GetDelegate<System.Action<bool>>(L, 2);
                    string _bundleRootDir = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.InitBundleLoader( _initCallback, _bundleRootDir );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeMaxLoadingCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _count = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeMaxLoadingCount( _count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadAssetAsync( _address, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadAssetAsync( _address, _progress, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    object _userData = translator.GetObject(L, 4, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadAssetAsync( _address, _complete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 5, out _priority);
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadAssetAsync( _address, _progress, _complete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.LoadAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstanceAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceAssetAsync( _address, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceAssetAsync( _address, _progress, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    object _userData = translator.GetObject(L, 4, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceAssetAsync( _address, _complete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 5, out _priority);
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceAssetAsync( _address, _progress, _complete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.InstanceAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadBatchAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsync( _addresses, _batchComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsync( _addresses, _complete, _batchComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    object _userData = translator.GetObject(L, 4, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsync( _addresses, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    object _userData = translator.GetObject(L, 5, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsync( _addresses, _complete, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 5, out _priority);
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsync( _addresses, _complete, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 7)&& translator.Assignable<object>(L, 8)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    DotEngine.Asset.OnBatchAssetsLoadProgress _batchProgress = translator.GetDelegate<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 7, out _priority);
                    object _userData = translator.GetObject(L, 8, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsync( _addresses, _progress, _complete, _batchProgress, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.LoadBatchAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadBatchAssetAsyncByLabel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsyncByLabel( _label, _batchComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    object _userData = translator.GetObject(L, 4, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsyncByLabel( _label, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    object _userData = translator.GetObject(L, 5, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsyncByLabel( _label, _complete, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 5, out _priority);
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsyncByLabel( _label, _complete, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 7)&& translator.Assignable<object>(L, 8)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    DotEngine.Asset.OnBatchAssetsLoadProgress _batchProgress = translator.GetDelegate<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 7, out _priority);
                    object _userData = translator.GetObject(L, 8, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.LoadBatchAssetAsyncByLabel( _label, _progress, _complete, _batchProgress, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.LoadBatchAssetAsyncByLabel!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstanceBatchAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsync( _addresses, _batchComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsync( _addresses, _complete, _batchComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    object _userData = translator.GetObject(L, 4, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsync( _addresses, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    object _userData = translator.GetObject(L, 5, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsync( _addresses, _complete, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 5, out _priority);
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsync( _addresses, _complete, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& translator.Assignable<string[]>(L, 2)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 7)&& translator.Assignable<object>(L, 8)) 
                {
                    string[] _addresses = (string[])translator.GetObject(L, 2, typeof(string[]));
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    DotEngine.Asset.OnBatchAssetsLoadProgress _batchProgress = translator.GetDelegate<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 7, out _priority);
                    object _userData = translator.GetObject(L, 8, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsync( _addresses, _progress, _complete, _batchProgress, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.InstanceBatchAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstanceBatchAssetAsyncByLabel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsyncByLabel( _label, _batchComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 3);
                    object _userData = translator.GetObject(L, 4, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsyncByLabel( _label, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    object _userData = translator.GetObject(L, 5, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsyncByLabel( _label, _complete, _batchComplete, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 3)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 3);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 4);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 5, out _priority);
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsyncByLabel( _label, _complete, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DotEngine.Asset.OnAssetLoadProgress>(L, 3)&& translator.Assignable<DotEngine.Asset.OnAssetLoadComplete>(L, 4)&& translator.Assignable<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5)&& translator.Assignable<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6)&& translator.Assignable<DotEngine.Asset.AssetLoaderPriority>(L, 7)&& translator.Assignable<object>(L, 8)) 
                {
                    string _label = LuaAPI.lua_tostring(L, 2);
                    DotEngine.Asset.OnAssetLoadProgress _progress = translator.GetDelegate<DotEngine.Asset.OnAssetLoadProgress>(L, 3);
                    DotEngine.Asset.OnAssetLoadComplete _complete = translator.GetDelegate<DotEngine.Asset.OnAssetLoadComplete>(L, 4);
                    DotEngine.Asset.OnBatchAssetsLoadProgress _batchProgress = translator.GetDelegate<DotEngine.Asset.OnBatchAssetsLoadProgress>(L, 5);
                    DotEngine.Asset.OnBatchAssetLoadComplete _batchComplete = translator.GetDelegate<DotEngine.Asset.OnBatchAssetLoadComplete>(L, 6);
                    DotEngine.Asset.AssetLoaderPriority _priority;translator.Get(L, 7, out _priority);
                    object _userData = translator.GetObject(L, 8, typeof(object));
                    
                        DotEngine.Asset.AssetHandler gen_ret = gen_to_be_invoked.InstanceBatchAssetAsyncByLabel( _label, _progress, _complete, _batchProgress, _batchComplete, _priority, _userData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.InstanceBatchAssetAsyncByLabel!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<DotEngine.Asset.AssetHandler>(L, 2)) 
                {
                    DotEngine.Asset.AssetHandler _handler = (DotEngine.Asset.AssetHandler)translator.GetObject(L, 2, typeof(DotEngine.Asset.AssetHandler));
                    
                    gen_to_be_invoked.UnloadAssetAsync( _handler );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<DotEngine.Asset.AssetHandler>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    DotEngine.Asset.AssetHandler _handler = (DotEngine.Asset.AssetHandler)translator.GetObject(L, 2, typeof(DotEngine.Asset.AssetHandler));
                    bool _destroyIfIsInstnace = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.UnloadAssetAsync( _handler, _destroyIfIsInstnace );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.UnloadAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstantiateAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _address = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Object _asset = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    
                        UnityEngine.Object gen_ret = gen_to_be_invoked.InstantiateAsset( _address, _asset );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadUnusedAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.Asset.AssetService gen_to_be_invoked = (DotEngine.Asset.AssetService)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Action>(L, 2)) 
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 2);
                    
                    gen_to_be_invoked.UnloadUnusedAsset( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.UnloadUnusedAsset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.Asset.AssetService.UnloadUnusedAsset!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
