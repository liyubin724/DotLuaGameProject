using System;
using System.Collections.Generic;
using XLua;

namespace DotEngine.Lua
{
    public class LuaLocalization : IDisposable
    {
        private Dictionary<string, string> cachedTextDic = new Dictionary<string, string>();
        private LuaTable languageTable = null;

        public LuaLocalization()
        {
        }

        public void ChangeLanguage(LuaTable language)
        {
            if (languageTable != null && languageTable != language)
            {
                languageTable.Dispose();
                languageTable = null;
            }
            languageTable = language;
            cachedTextDic.Clear();
        }

        public string GetText(string locName)
        {
            string text = string.Empty;
            if (languageTable != null)
            {
                if (!cachedTextDic.TryGetValue(locName, out text))
                {
                    text = languageTable.Get<string>(locName);
                    if (text == null)
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
            if (languageTable != null)
            {
                languageTable.Dispose();
                languageTable = null;
            }
            cachedTextDic.Clear();
        }
    }
}
