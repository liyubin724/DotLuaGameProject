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
    
    public class DotEngineAssetAssetLoaderPriorityWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DotEngine.Asset.AssetLoaderPriority), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DotEngine.Asset.AssetLoaderPriority), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DotEngine.Asset.AssetLoaderPriority), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "VeryLow", DotEngine.Asset.AssetLoaderPriority.VeryLow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Low", DotEngine.Asset.AssetLoaderPriority.Low);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", DotEngine.Asset.AssetLoaderPriority.Default);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "High", DotEngine.Asset.AssetLoaderPriority.High);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "VeryHigh", DotEngine.Asset.AssetLoaderPriority.VeryHigh);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DotEngine.Asset.AssetLoaderPriority), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDotEngineAssetAssetLoaderPriority(L, (DotEngine.Asset.AssetLoaderPriority)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "VeryLow"))
                {
                    translator.PushDotEngineAssetAssetLoaderPriority(L, DotEngine.Asset.AssetLoaderPriority.VeryLow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Low"))
                {
                    translator.PushDotEngineAssetAssetLoaderPriority(L, DotEngine.Asset.AssetLoaderPriority.Low);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Default"))
                {
                    translator.PushDotEngineAssetAssetLoaderPriority(L, DotEngine.Asset.AssetLoaderPriority.Default);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "High"))
                {
                    translator.PushDotEngineAssetAssetLoaderPriority(L, DotEngine.Asset.AssetLoaderPriority.High);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "VeryHigh"))
                {
                    translator.PushDotEngineAssetAssetLoaderPriority(L, DotEngine.Asset.AssetLoaderPriority.VeryHigh);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DotEngine.Asset.AssetLoaderPriority!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DotEngine.Asset.AssetLoaderPriority! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DotEngineUIUILayerLevelWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DotEngine.UI.UILayerLevel), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DotEngine.UI.UILayerLevel), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DotEngine.UI.UILayerLevel), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomlowestLayer", DotEngine.UI.UILayerLevel.BottomlowestLayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomLayer", DotEngine.UI.UILayerLevel.BottomLayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DefaultLayer", DotEngine.UI.UILayerLevel.DefaultLayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopLayer", DotEngine.UI.UILayerLevel.TopLayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopmostLayer", DotEngine.UI.UILayerLevel.TopmostLayer);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DotEngine.UI.UILayerLevel), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDotEngineUIUILayerLevel(L, (DotEngine.UI.UILayerLevel)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "BottomlowestLayer"))
                {
                    translator.PushDotEngineUIUILayerLevel(L, DotEngine.UI.UILayerLevel.BottomlowestLayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BottomLayer"))
                {
                    translator.PushDotEngineUIUILayerLevel(L, DotEngine.UI.UILayerLevel.BottomLayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DefaultLayer"))
                {
                    translator.PushDotEngineUIUILayerLevel(L, DotEngine.UI.UILayerLevel.DefaultLayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopLayer"))
                {
                    translator.PushDotEngineUIUILayerLevel(L, DotEngine.UI.UILayerLevel.TopLayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopmostLayer"))
                {
                    translator.PushDotEngineUIUILayerLevel(L, DotEngine.UI.UILayerLevel.TopmostLayer);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DotEngine.UI.UILayerLevel!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DotEngine.UI.UILayerLevel! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineSystemLanguageWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.SystemLanguage), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.SystemLanguage), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.SystemLanguage), L, null, 45, 0, 0);

            Utils.RegisterEnumType(L, typeof(UnityEngine.SystemLanguage));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.SystemLanguage), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineSystemLanguage(L, (UnityEngine.SystemLanguage)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(UnityEngine.SystemLanguage), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(UnityEngine.SystemLanguage) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.SystemLanguage! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}