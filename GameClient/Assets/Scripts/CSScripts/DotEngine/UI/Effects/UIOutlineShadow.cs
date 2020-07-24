﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI.Effects
{
    [AddComponentMenu("UI/Effects/OutlineShadow", 21)]
    [RequireComponent(typeof(Graphic))]
    public class UIOutlineShadow : BaseMeshEffect
    {
        [SerializeField]
        private bool m_EnableOutline = false;

        [SerializeField]
        private Color m_OutlineEffectColor = new Color(0f, 0f, 0f, 0.5f);

        [SerializeField]
        private Vector2 m_OutlineEffectDistance = new Vector2(1f, -1f);

        [SerializeField]
        private bool m_OutlineUseGraphicAlpha = true;

        [SerializeField]
        private bool m_EnableShadow = false;

        [SerializeField]
        private Color m_ShadowEffectColor = new Color(0f, 0f, 0f, 0.5f);

        [SerializeField]
        private Vector2 m_ShadowEffectDistance = new Vector2(1f, -1f);

        [SerializeField]
        private bool m_ShadowUseGraphicAlpha = true;

        private const float kMaxEffectDistance = 600f;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            outlineEffectDistance = m_OutlineEffectDistance;
            shadowEffectDistance = m_ShadowEffectDistance;
            base.OnValidate();
        }

#endif

        public bool EnableOutline
        {
            get { return m_EnableOutline; }
            set
            {
                if (m_EnableOutline != value)
                {
                    m_EnableOutline = value;
                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        public Color outlineEffectColor
        {
            get { return m_OutlineEffectColor; }
            set
            {
                m_OutlineEffectColor = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public Vector2 outlineEffectDistance
        {
            get { return m_OutlineEffectDistance; }
            set
            {
                value = DistanceLimit(value);

                if (m_OutlineEffectDistance == value)
                    return;

                m_OutlineEffectDistance = value;

                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public bool outlineUseGraphicAlpha
        {
            get { return m_OutlineUseGraphicAlpha; }
            set
            {
                m_OutlineUseGraphicAlpha = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public bool EnableShadow
        {
            get { return m_EnableShadow; }
            set
            {
                if (m_EnableShadow != value)
                {
                    m_EnableShadow = value;
                    if (graphic != null)
                    {
                        graphic.SetVerticesDirty();
                    }
                }
            }
        }

        public Color shadowEffectColor
        {
            get { return m_ShadowEffectColor; }
            set
            {
                m_ShadowEffectColor = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public Vector2 shadowEffectDistance
        {
            get { return m_ShadowEffectDistance; }
            set
            {
                value = DistanceLimit(value);

                if (m_ShadowEffectDistance == value)
                    return;

                m_ShadowEffectDistance = value;

                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public bool shadowUseGraphicAlpha
        {
            get { return m_ShadowUseGraphicAlpha; }
            set
            {
                m_ShadowUseGraphicAlpha = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        /// 限定距离最大最小尺寸
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Vector2 DistanceLimit(Vector2 value)
        {
            if (value.x > kMaxEffectDistance)
                value.x = kMaxEffectDistance;
            if (value.x < -kMaxEffectDistance)
                value.x = -kMaxEffectDistance;

            if (value.y > kMaxEffectDistance)
                value.y = kMaxEffectDistance;
            if (value.y < -kMaxEffectDistance)
                value.y = -kMaxEffectDistance;

            return value;
        }

        /// <summary>
        /// 绘制单个Shadow
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="color"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="useGraphicAlpha"></param>
        protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y, bool useGraphicAlpha)
        {
            UIVertex vt;

            var neededCapacity = verts.Count + end - start;
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x;
                v.y += y;
                vt.position = v;
                var newColor = color;
                if (useGraphicAlpha)
                    newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            var output = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(output);

            int len = output.Count;

            var neededCpacity = output.Count;
            if (m_EnableShadow)
            {
                neededCpacity += output.Count;
            }
            if (m_EnableOutline)
            {
                neededCpacity += output.Count * 4;
            }
            if (output.Capacity < neededCpacity)
                output.Capacity = neededCpacity;

            int start = 0;
            int end = output.Count;
            //Shadow
            if (m_EnableShadow)
            {
                ApplyShadowZeroAlloc(output, shadowEffectColor, start, end, shadowEffectDistance.x, shadowEffectDistance.y, m_ShadowUseGraphicAlpha);
                start = end;
                end = output.Count;
            }

            //Outline
            if (m_EnableOutline)
            {
                ApplyShadowZeroAlloc(output, outlineEffectColor, start, output.Count, outlineEffectDistance.x, outlineEffectDistance.y, m_OutlineUseGraphicAlpha);
                start = end;
                end = output.Count;

                ApplyShadowZeroAlloc(output, outlineEffectColor, start, output.Count, outlineEffectDistance.x, -outlineEffectDistance.y, m_OutlineUseGraphicAlpha);
                start = end;
                end = output.Count;

                ApplyShadowZeroAlloc(output, outlineEffectColor, start, output.Count, -outlineEffectDistance.x, outlineEffectDistance.y, m_OutlineUseGraphicAlpha);
                start = end;
                end = output.Count;

                ApplyShadowZeroAlloc(output, outlineEffectColor, start, output.Count, -outlineEffectDistance.x, -outlineEffectDistance.y, m_OutlineUseGraphicAlpha);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
            ListPool<UIVertex>.Release(output);
        }
    }
}
