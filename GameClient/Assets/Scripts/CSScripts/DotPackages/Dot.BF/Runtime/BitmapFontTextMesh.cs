using UnityEngine;

namespace DotEngine.Fonts
{
    [RequireComponent(typeof(TextMesh))]
    [ExecuteInEditMode]
    public class BitmapFontTextMesh : BitmapFontText
    {
        public TextMesh textMesh = null;
        public MeshRenderer meshRenderer = null;

        protected override void OnTextChanged(string mappedText)
        {
            if(textMesh == null || meshRenderer == null)
            {
                Debug.LogError("BitmapFontTextMesh::OnTextChanged->the field(textMesh/meshRenderer) is empty!");
            }
            else
            {
                if (FontData.bmFont != null && textMesh.font != FontData.bmFont)
                {
                    textMesh.font = FontData.bmFont;
                    meshRenderer.sharedMaterial= FontData.bmFont.material;
                }
                textMesh.text = mappedText;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (textMesh != null)
            {
                if (FontData != null && FontData.bmFont != null)
                {
                    if(textMesh.font!=FontData.bmFont)
                    {
                        textMesh.font = FontData.bmFont;
                    }
                }
                else
                {
                    textMesh.font = null;
                }
            }

            if(meshRenderer!=null)
            {
                if (FontData != null && FontData.bmFont != null && FontData.bmFont.material!=null)
                {
                    if(FontData.bmFont.material != meshRenderer.sharedMaterial)
                    {
                        meshRenderer.sharedMaterial = FontData.bmFont.material;
                    }
                }
                else
                {
                    meshRenderer.material = null;
                }
            }

            base.OnValidate();
        }
#endif
    }
}
