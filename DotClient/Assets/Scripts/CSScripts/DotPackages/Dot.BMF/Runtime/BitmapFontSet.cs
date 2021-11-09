using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DotEngine.BMF
{
    public class BitmapFontSet : ScriptableObject,ISerializationCallbackReceiver
    {
        [SerializeField]
        private Font m_Font = null;
        public Font Font
        {
            get
            {
                return m_Font;
            }
        }

        [SerializeField]
        private List<BitmapFont> m_BMFonts = new List<BitmapFont>();

        private Dictionary<string, Dictionary<char, char>> m_MappedFontCharDic = new Dictionary<string, Dictionary<char, char>>();
        private StringBuilder m_TMPStrBuilder = new StringBuilder();

        public void AddText(string fontName,char originChar,char mappedChar)
        {
            if(!m_MappedFontCharDic.TryGetValue(fontName,out var charDic))
            {
                charDic = new Dictionary<char, char>();
                m_MappedFontCharDic.Add(fontName, charDic);
            }
            if(!charDic.ContainsKey(originChar))
            {
                charDic.Add(originChar, mappedChar);
            }else
            {
                Debug.LogWarning("the char has been added to the map");
            }
        }

        public void OnAfterDeserialize()
        {
            m_MappedFontCharDic.Clear();
            for(int i =0;i<m_BMFonts.Count;++i)
            {
                BitmapFont bmFont = m_BMFonts[i];

                Dictionary<char, char> charDic = new Dictionary<char, char>();
                m_MappedFontCharDic.Add(bmFont.Name, charDic);

                foreach(var fontChar in bmFont.Chars)
                {
                    charDic.Add(fontChar.Origin, fontChar.Mapped);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            m_BMFonts.Clear();
            foreach(var fontKvp in m_MappedFontCharDic)
            {
                BitmapFont bmFont = new BitmapFont();
                bmFont.Name = fontKvp.Key;
                foreach(var charKvp in fontKvp.Value)
                {
                    bmFont.Chars.Add(new BitmapFontChar()
                    {
                        Origin = charKvp.Key,
                        Mapped = charKvp.Value,
                    });
                }

                m_BMFonts.Add(bmFont);
            }
        }

        public string GetText(string fontName, string text)
        {
            if (string.IsNullOrEmpty(fontName) || string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if(!m_MappedFontCharDic.TryGetValue(fontName,out var charDic))
            {
                return string.Empty;
            }

            m_TMPStrBuilder.Clear();
            for(int i =0;i<text.Length;++i)
            {
                if(charDic.TryGetValue(text[i],out var mappedChar))
                {
                    m_TMPStrBuilder.Append(mappedChar);
                }else
                {
                    m_TMPStrBuilder.Append(" ");
                }
            }

            return m_TMPStrBuilder.ToString();
        }

        [Serializable]
        public class BitmapFont
        {
            public string Name = string.Empty;
            public List<BitmapFontChar> Chars = new List<BitmapFontChar>();
        }

        [Serializable]
        public class BitmapFontChar
        {
            public char Origin;
            public char Mapped;
        }
    }
}
