using DotEngine.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Services
{
    public class LuaHandlerService : Service
    {
        private LuaBindScript m_BindScript = null;

        public LuaHandlerService(string name,string envName,string scriptPath) : base(name)
        {
            m_BindScript = new LuaBindScript(envName, scriptPath);
        }

        public override void DoRegister()
        {
            m_BindScript.InitLua(null);
        }

        public override void DoRemove()
        {
            
        }
    }
}
