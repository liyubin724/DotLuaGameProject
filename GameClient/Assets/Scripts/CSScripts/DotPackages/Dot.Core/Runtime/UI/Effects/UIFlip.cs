using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI.Effects
{
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    [AddComponentMenu("UI/Effects/Flip", 22)]
    public class UIFlip : BaseMeshEffect
    {
        [Tooltip("Flip horizontally.")]
        [SerializeField] private bool m_Horizontal = false;

        [Tooltip("Flip vertically.")]
        [SerializeField] private bool m_Veritical = false;

        public bool horizontal { get { return this.m_Horizontal; } set { this.m_Horizontal = value; } }
        public bool vertical { get { return this.m_Veritical; } set { this.m_Veritical = value; } }

        public override void ModifyMesh(VertexHelper vh)
        {
            RectTransform rt = graphic.rectTransform;
            UIVertex vt = default(UIVertex);
            Vector3 pos;
            Vector2 center = rt.rect.center;
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vt, i);
                pos = vt.position;
                vt.position = new Vector3(
                    m_Horizontal ? -pos.x : pos.x,
                    m_Veritical ? -pos.y : pos.y
                //					m_Horizontal ? (pos.x + (center.x - pos.x) * 2) : pos.x,
                //					m_Veritical ? (pos.y + (center.y - pos.y) * 2) : pos.y
                );
                vh.SetUIVertex(vt, i);
            }
        }
    }
}
