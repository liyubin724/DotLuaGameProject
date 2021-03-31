using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace DotEngine.Lua.Localization
{
    public class LuaLocalLanguage : IDisposable
    {

        public event Action OnLanguageChanged = null;

        private Dictionary<string, string> cachedTextDic = new Dictionary<string, string>();
        private LuaTable languageTable = null;

        public LuaLocalLanguage(LuaTable language)
        {
            languageTable = language;
        }

        public void ChangeLanguage(LuaTable language)
        {
            if(languageTable!=null && languageTable != language)
            {
                languageTable.Dispose();
                languageTable = null;
            }
            languageTable = language;
            cachedTextDic.Clear();
            OnLanguageChanged?.Invoke();
        }

        public string GetText(string locName)
        {
            string text = string.Empty;
            if (languageTable != null)
            {
                if(!cachedTextDic.TryGetValue(locName,out text))
                {
                    text = languageTable.Get<string>(locName);
                    if(text == null)
                    {
                        text = string.Empty;
                    }
                    cachedTextDic.Add(locName, text);
                }
            }
            return text;
        }

        public void Dispose()
        {
            if(languageTable!=null)
            {
                languageTable.Dispose();
                languageTable = null;
            }
            cachedTextDic.Clear();
            OnLanguageChanged = null;
        }
    }
}
