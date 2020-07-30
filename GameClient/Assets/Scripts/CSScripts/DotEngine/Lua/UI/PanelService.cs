using DotEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Lua.UI
{


    public class PanelService : LuaHandlerService
    {
        private const string NAME = "PanelService";

        public PanelService() : base(NAME, "PanelMgr", "DotLua/UI/UIPanelManager")
        {
        }


    }
}
