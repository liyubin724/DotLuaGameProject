using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Lua)]
    public class LuaField : WDBField
    {
        public LuaField(int col) : base(col)
        {
        }
    }
}
