using DotEngine.NativeDrawer.Property;
using DotEngine.NativeDrawer.Visible;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEditor.Fonts
{
    public class BitmapFontConfig : ScriptableObject
    {
        [OpenFolderPath]
        public string outputAssetDir = string.Empty;
        [IntSlider(LeftValue =2,RightValue =10)]
        public int padding = 2;
        [IntPopup(Values =new int[] { 64,128,256,512,1024,2048},Contents = new string[] { "64 x 64", "128 x 128", "256 x 256", "512 x 512", "1024 x 1024", "2048 x 2048" })]
        public int maxSize = 1024;

        public List<BitmapFontChar> fontChars = new List<BitmapFontChar>();

        public string GetFontPath()
        {
            return $"{outputAssetDir}/{name}_bmf_font.asset";
        }

        public string GetFontDataPath()
        {
            return $"{outputAssetDir}/{name}_bmf_data.asset";
        }

        public string GetFontTexturePath()
        {
            return $"{outputAssetDir}/{name}_bmf_tex.png";
        }

        public bool IsValid()
        {
            if(string.IsNullOrEmpty(outputAssetDir) || !outputAssetDir.StartsWith("Assets"))
            {
                return false;
            }

            if(fontChars == null ||fontChars.Count ==0)
            {
                return false;
            }

            bool isRepeat = fontChars.GroupBy((fc) => fc.fontName).Where((g) => g.Count() > 1).Count() > 0;
            if (isRepeat)
            {
                return false;
            }

            foreach (var fontChar in fontChars)
            {
                if(!fontChar.IsValid())
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return name;
        }

        [Serializable]
        public class BitmapFontChar
        {
            public string fontName = string.Empty;
            public int charSpace = 0;
            
            public List<char> chars = new List<char>();
            public List<Texture2D> textures = new List<Texture2D>();

            [NonSerialized]
            [Hide]
            public int[] charIndexes = new int[0];

            [NonSerialized]
            [Hide]
            public Rect[] charRects = new Rect[0];

            public bool IsValid()
            {
                if(string.IsNullOrEmpty(fontName))
                {
                    return false;
                }
                if(chars == null || chars.Count == 0)
                {
                    return false;
                }
                if(textures == null || textures.Count == 0)
                {
                    return false;
                }
                if(chars.Count!=textures.Count)
                {
                    return false;
                }

                bool isRepeat = chars.GroupBy((c) => c).Where((g)=>g.Count()>1).Count()>0 ;
                if(isRepeat)
                {
                    return false;
                }
                bool isNull = textures.Any((texture) => texture == null);
                if(isNull)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
