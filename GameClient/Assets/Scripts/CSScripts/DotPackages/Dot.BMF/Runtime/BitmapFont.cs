using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DotEngine.BMF
{
    public class BitmapFont : ScriptableObject
    {
        public Font bmFont = null;
        public BitmapFontCharMap[] charMaps = new BitmapFontCharMap[0];

        private Dictionary<string, BitmapFontCharMap> charMapDic = null;
        private StringBuilder textBuilder = new StringBuilder();

        public string GetText(string fontName,string text)
        {
            if(string.IsNullOrEmpty(fontName) || string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if(charMapDic == null)
            {
                charMapDic = new Dictionary<string, BitmapFontCharMap>();
                for(int i =0;i<charMaps.Length;++i)
                {
                    charMapDic.Add(charMaps[i].name, charMaps[i]);
                }
            }

            if(!charMapDic.TryGetValue(fontName,out BitmapFontCharMap charMap))
            {
                return string.Empty;
            }
            textBuilder.Clear();
            for(int i =0;i<text.Length;++i)
            {
                textBuilder.Append(charMap.GetChar(text[i]));
            }
            return textBuilder.ToString();
        }

        [Serializable]
        public class BitmapFontCharMap
        {
            public string name = string.Empty;
            public char[] orgChars = new char[0];
            public char[] mapChars = new char[0];

            private Dictionary<char, char> charDic = null;
            public char GetChar(char c)
            {
                if(charDic == null)
                {
                    charDic = new Dictionary<char, char>();
                    for (int i = 0; i < orgChars.Length; ++i)
                    {
                        charDic.Add(orgChars[i], mapChars[i]);
                    }
                }
                if (charDic.TryGetValue(c, out char result))
                {
                    return result;
                }
                return ' ';
            }
        }
    }
}
