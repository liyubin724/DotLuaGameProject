using DotEngine.Lua;

namespace DotEngine.Services
{
    public class LuaHandlerService : Service
    {
        protected LuaBindScript bindScript = null;
        private string m_HandlerName = string.Empty;

        public LuaHandlerService(string service, string handler, string scriptPath) : base(service)
        {
            bindScript = new LuaBindScript(scriptPath);
            m_HandlerName = handler;
        }

        public override void DoRegister()
        {
            bindScript.InitLua();
            if(bindScript.IsValid())
            {
                LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
                service.GlobalGameTable.Set(m_HandlerName, bindScript.ObjTable);

                bindScript.CallAction(LuaConst.START_FUNCTION_NAME);
            }
        }

        public override void DoRemove()
        {
            if(bindScript.IsValid())
            {
                bindScript.CallAction(LuaConst.DESTROY_FUNCTION_NAME);
                bindScript.Dispose();
            }
            bindScript = null;
        }

        public void CallAction<T>(string funcName,T value)
        {
            bindScript.CallAction<T>(funcName, value);
        }
    }
}
