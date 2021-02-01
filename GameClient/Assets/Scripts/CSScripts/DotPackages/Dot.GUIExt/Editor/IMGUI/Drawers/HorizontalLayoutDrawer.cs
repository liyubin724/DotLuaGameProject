using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
{
    public class HorizontalLayoutDrawer : ILayoutDrawable
    {
        public GUIStyle Style { get; set; } = GUIStyle.none;

        private List<ILayoutDrawable> items = new List<ILayoutDrawable>();

        public HorizontalLayoutDrawer(params ILayoutDrawable[] items)
        {
            if (items != null && items.Length > 0)
            {
                this.items.AddRange(items);
            }
        }

        public HorizontalLayoutDrawer Add(ILayoutDrawable item)
        {
            items.Add(item);
            return this;
        }

        public HorizontalLayoutDrawer Remove(ILayoutDrawable item)
        {
            items.Remove(item);
            return this;
        }

        public HorizontalLayoutDrawer Clear()
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
