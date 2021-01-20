using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class VerticalCompositeDrawer : ILayoutDrawable
    {
        public GUIStyle Style { get; set; } = GUIStyle.none;

        private List<ILayoutDrawable> items = new List<ILayoutDrawable>();

        public VerticalCompositeDrawer(params ILayoutDrawable[] items)
        {
            if(items!=null && items.Length>0)
            {
                this.items.AddRange(items);
            }
        }

        public VerticalCompositeDrawer Add(ILayoutDrawable item)
        {
            items.Add(item);
            return this;
        }

        public VerticalCompositeDrawer Remove(ILayoutDrawable item)
        {
            items.Remove(item);
            return this;
        }

        public VerticalCompositeDrawer Clear()
        {
            items.Clear();
            return this;
        }

        public void OnGUILayout()
        {
            EditorGUILayout.BeginVertical(Style);
            {
                foreach (var item in items)
                {
                    item?.OnGUILayout();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
