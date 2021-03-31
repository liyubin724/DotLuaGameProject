using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    public class LuaLocalizationText : Text
    {
        public string localizationName = null;

        protected override void Awake()
        {
            if(Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                LuaEnvManager envMgr = LuaEnvManager.GetInstance();
                if(envMgr!=null)
                {
                    text = envMgr.Language.GetText(localizationName);
                }
            }
            base.Awake();
        }
    }
}
