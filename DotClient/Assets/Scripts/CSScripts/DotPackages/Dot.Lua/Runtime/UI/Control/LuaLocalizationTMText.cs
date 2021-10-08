using TMPro;
using UnityEngine;

namespace DotEngine.Lua.UI
{
    //TMP_EditorPanelUI
    public class LuaLocalizationTMText : TextMeshProUGUI
    {
        public string bridgerName = null;
        public string localizationName = null;

        protected override void Awake()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                LuaBridger bridger = LuaManager.GetInstance().GetBridger(bridgerName);
                bridger.OnLanguageChanged += OnLanguageChanged;

                OnLanguageChanged();
            }
            base.Awake();
        }

        protected override void OnDestroy()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(localizationName))
            {
                LuaBridger bridger = LuaManager.GetInstance().GetBridger(bridgerName);
                bridger.OnLanguageChanged -= OnLanguageChanged;
            }
            base.OnDestroy();
        }

        private void OnLanguageChanged()
        {
            LuaBridger bridger = LuaManager.GetInstance().GetBridger(bridgerName);
            text = bridger.GetLocalizationText(localizationName);
        }

    }
}
