using System.Collections.Generic;
using XLua;

namespace DotEngine.Lua.Binder
{
    public class LuaRefBehaviour : LuaBehaviour
    {
        public List<LuaParam> registParamList = new List<LuaParam>();
        public List<LuaParams> registerParamsList = new List<LuaParams>();

        protected override void OnInitFinished()
        {
            base.OnInitFinished();

            foreach (var p in registParamList)
            {
                RegisterParamToTable(Table, p.name, p.value);
            }

            foreach (var p in registerParamsList)
            {
                LuaTable regTable = Env.NewTable();
                Table.Set(p.name, regTable);

                foreach (var v in p.values)
                    for (int i = 0; i < p.values.Count; ++i)
                    {
                        RegisterParamToTable(regTable, i, p.values[i]);
                    }
            }
        }

        private void RegisterParamToTable(LuaTable table, string name, LuaParamValue paramValue)
        {
            if (string.IsNullOrEmpty(name) || !IsValid() || paramValue == null)
            {
                return;
            }

            SetValue(name, paramValue.GetValue());
        }

        private void RegisterParamToTable(LuaTable table, int index, LuaParamValue paramValue)
        {
            if (!IsValid() || paramValue == null)
            {
                return;
            }

            SetValue(index, paramValue.GetValue());
        }
    }
}
