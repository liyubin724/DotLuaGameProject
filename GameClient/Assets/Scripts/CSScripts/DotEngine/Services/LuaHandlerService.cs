using DotEngine.Lua;

namespace DotEngine.Services
{
    public class LuaHandlerService : Service
    {
        private LuaBindScript m_BindScript = null;

        public LuaHandlerService(string name,string scriptPath) : base(name)
        {
            m_BindScript = new LuaBindScript(scriptPath);
        }

        public override void DoRegister()
        {
            m_BindScript.InitLua();
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
