using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

namespace DotEngine.Lua
{
    public class LuaLauncher
    {
        private static readonly string LUA_LAUNCH_NAME = "Launcher";
        private static readonly string LUA_LAUNCH_PATH = "DotLua/Launcher";

        private static readonly string LUA_LAUNCH_INIT_FUNC_NAME = "DoInit";
        private static readonly string LUA_LAUNCH_UPDATE_FUNC_NAME = "DoUpdate";
        private static readonly string LUA_LAUNCH_LATEUPDATE_FUNC_NAME = "DoLateUpdate";
        private static readonly string LUA_LAUNCH_TEARDOWN_FUNC_NAME = "DoTeardown";

        public LuaEnv Env { get; private set; } = null;
        private ScriptLoader scriptLoader = null;
        public LuaTable Launcher { get; private set; } = null;

        private Action<float, float> updateAction = null;
        private Action lateUpdateAction = null;
        private Action teardownAction = null;

        public void DoStartup()
        {
            Env = new LuaEnv();
            scriptLoader = new FileScriptLoader()
        }

        public void DoUpdate()
        {

        }

        public void DoLateUpdate()
        {

        }

        public void DoDestroy()
        {

        }


    }
}
