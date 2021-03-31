using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DotEngine.Lua.UI
{
    //TMP_EditorPanelUI
    public class LuaLocalizationTMText : TextMeshProUGUI
    {
        public string localizationName = null;

        protected override void Awake()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                LuaEnvManager envMgr = LuaEnvManager.GetInstance();
                if (envMgr != null)
                {
                    text = envMgr.Language.GetText(localizationName);
                }
            }

            base.Awake();
        }
    }
}
