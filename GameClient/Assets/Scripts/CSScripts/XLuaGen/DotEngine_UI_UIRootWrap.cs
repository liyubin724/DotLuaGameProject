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
    public class DotEngineUIUIRootWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(DotEngine.UI.UIRoot);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLayer", _m_GetLayer);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "uiCamera", _g_get_uiCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layers", _g_get_layers);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "uiCamera", _s_set_uiCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layers", _s_set_layers);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Root", _g_get_Root);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Root", _s_set_Root);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					DotEngine.UI.UIRoot gen_ret = new DotEngine.UI.UIRoot();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to DotEngine.UI.UIRoot constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLayer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                DotEngine.UI.UIRoot gen_to_be_invoked = (DotEngine.UI.UIRoot)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    DotEngine.UI.UILayerLevel _layerLevel;translator.Get(L, 2, out _layerLevel);
                    
                        DotEngine.UI.UILayer gen_ret = gen_to_be_invoked.GetLayer( _layerLevel );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Root(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, DotEngine.UI.UIRoot.Root);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_uiCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                DotEngine.UI.UIRoot gen_to_be_invoked = (DotEngine.UI.UIRoot)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.uiCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                DotEngine.UI.UIRoot gen_to_be_invoked = (DotEngine.UI.UIRoot)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layers);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Root(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    DotEngine.UI.UIRoot.Root = (DotEngine.UI.UIRoot)translator.GetObject(L, 1, typeof(DotEngine.UI.UIRoot));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_uiCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                DotEngine.UI.UIRoot gen_to_be_invoked = (DotEngine.UI.UIRoot)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.uiCamera = (DotEngine.UI.UICamera)translator.GetObject(L, 2, typeof(DotEngine.UI.UICamera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                DotEngine.UI.UIRoot gen_to_be_invoked = (DotEngine.UI.UIRoot)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layers = (DotEngine.UI.UILayer[])translator.GetObject(L, 2, typeof(DotEngine.UI.UILayer[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
