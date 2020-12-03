using DotEngine.Fonts;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class BitmapFontUIText : BitmapFontText
    {
        public Text uiText = null;

        protected override void OnTextChanged(string mappedText)
        {
            if (uiText == null)
            {
                Debug.LogError("BitmapFontUIText::OnTextChanged->the field(uiText) is empty!");
            }
            else
            {
                if (FontData.bmFont != null && uiText.font != FontData.bmFont)
                {
                    uiText.font = FontData.bmFont;
                }
                uiText.text = mappedText;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if(uiText!=null)
            {
                if (FontData != null && FontData.bmFont != null)
                {
                    if(uiText.font != FontData.bmFont)
                    {
                        uiText.font = FontData.bmFont;
                    }
                }else
                {
                    uiText.font = null;
                }
            }
            base.OnValidate();
        }
#endif
    }
}


