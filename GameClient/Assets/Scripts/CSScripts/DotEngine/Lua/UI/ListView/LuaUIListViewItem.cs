using DotEngine.Lua.Register;
using SuperScrollView;
using UnityEngine;
using XLua;

namespace DotEngine.Lua.UI.ListView
{
    [RequireComponent(typeof(LoopListViewItem2))]
    public class LuaUIListViewItem : LoopListViewItem2
    {
        private const string ITEM_NAME = "viewItem";

        public LuaTable ObjTable
        {
            get
            {
                return bindScript.ObjTable;
            }
        }

        public LuaBindScript bindScript = new LuaBindScript();
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected virtual void Awake()
        {
            bindScript.InitLua(OnInitFinished);
            bindScript.CallAction(LuaConst.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            bindScript.CallAction(LuaConst.START_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if(bindScript.IsValid())
            {
                bindScript.CallAction(LuaConst.DESTROY_FUNCTION_NAME);
                bindScript.Dispose();
            }
        }

        protected virtual void OnInitFinished()
        {
            LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
            if(service.IsValid())
            {
                registerObjectData.RegisterToLua(service.Env, bindScript.ObjTable);
                registerBehaviourData.RegisterToLua(service.Env, bindScript.ObjTable);

                bindScript.ObjTable.Set(ITEM_NAME, this);
            }
            
        }
    }
}
