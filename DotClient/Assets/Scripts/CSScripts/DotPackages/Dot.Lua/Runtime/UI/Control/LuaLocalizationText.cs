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
                text = LuaBridger.GetInstance().GetLocalizationText(localizationName);

                LuaBridger.GetInstance().OnLanguageChanged += OnLanguageChanged;
            }
            base.Awake();
        }

        protected override void OnDestroy()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                LuaBridger.GetInstance().OnLanguageChanged -= OnLanguageChanged;
            }
            base.OnDestroy();
        }

        private void OnLanguageChanged()
        {
            text = LuaBridger.GetInstance().GetLocalizationText(localizationName);
        }
    }
}
