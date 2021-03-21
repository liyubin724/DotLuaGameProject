using DotEngine.Lua;
using DotEngine.Lua.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETest
{
    public class TestLuaOperateParam : MonoBehaviour
    {
        public LuaScriptBinder scriptBinder = new LuaScriptBinder();
        public LuaOperateParam loParam = new LuaOperateParam();
    }
}
