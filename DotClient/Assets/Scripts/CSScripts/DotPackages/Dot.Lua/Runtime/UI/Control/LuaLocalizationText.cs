using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    public class LuaLocalizationText : Text
    {
        public string localizationName = null;

        protected override void Awake()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                text = LuaEnvManager.GetInstance().GetLocalizationText(localizationName);

                LuaEnvManager.GetInstance().OnLanguageChanged += OnLanguageChanged;
            }
            base.Awake();
        }

        protected override void OnDestroy()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                LuaEnvManager.GetInstance().OnLanguageChanged -= OnLanguageChanged;
            }
            base.OnDestroy();
        }

        private void OnLanguageChanged()
        {
            text = LuaEnvManager.GetInstance().GetLocalizationText(localizationName);
        }
    }
}
