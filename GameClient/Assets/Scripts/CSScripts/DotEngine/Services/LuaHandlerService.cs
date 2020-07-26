using DotEngine.Lua;

namespace DotEngine.Services
{
    public class LuaHandlerService : Service
    {
        private LuaBindScript m_BindScript = null;
        private string m_HandlerName = string.Empty;

        public LuaHandlerService(string service, string handler, string scriptPath) : base(service)
        {
            m_BindScript = new LuaBindScript(scriptPath);
            m_HandlerName = handler;
        }

        public override void DoRegister()
        {
            m_BindScript.InitLua();
            if(m_BindScript.IsValid())
            {
                LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
                service.GlobalGameTable.Set(m_HandlerName, m_BindScript.ObjTable);
            }
        }

        public override void DoRemove()
        {
            m_BindScript.Dispose();
        }

        public void CallAction<T>(string funcName,T value)
        {
            m_BindScript.CallAction<T>(funcName, value);
        }
    }
}
