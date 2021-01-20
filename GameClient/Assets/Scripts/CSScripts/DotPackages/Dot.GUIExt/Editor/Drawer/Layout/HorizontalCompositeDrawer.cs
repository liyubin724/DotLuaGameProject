using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class HorizontalCompositeDrawer : ILayoutDrawable
    {
        public GUIStyle Style { get; set; } = GUIStyle.none;

        private List<ILayoutDrawable> items = new List<ILayoutDrawable>();

        public HorizontalCompositeDrawer(params ILayoutDrawable[] items)
        {
            if (items != null && items.Length > 0)
            {
                this.items.AddRange(items);
            }
        }

        public HorizontalCompositeDrawer Add(ILayoutDrawable item)
        {
            items.Add(item);
            return this;
        }

        public HorizontalCompositeDrawer Remove(ILayoutDrawable item)
        {
            items.Remove(item);
            return this;
        }

        public HorizontalCompositeDrawer Clear()
        {
            items.Clear();
            return this;
        }

        public void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(Style);
            {
                foreach(var item in items)
                {
                    item?.OnGUILayout();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
