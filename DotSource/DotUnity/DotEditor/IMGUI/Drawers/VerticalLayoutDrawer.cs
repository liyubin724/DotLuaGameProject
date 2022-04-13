using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
{
    public class VerticalLayoutDrawer : ILayoutDrawable
    {
        public GUIStyle Style { get; set; } = GUIStyle.none;

        private List<ILayoutDrawable> items = new List<ILayoutDrawable>();

        public VerticalLayoutDrawer(params ILayoutDrawable[] items)
        {
            if(items!=null && items.Length>0)
            {
                this.items.AddRange(items);
            }
        }

        public VerticalLayoutDrawer Add(ILayoutDrawable item)
        {
            items.Add(item);
            return this;
        }

        public VerticalLayoutDrawer Remove(ILayoutDrawable item)
        {
            items.Remove(item);
            return this;
        }

        public VerticalLayoutDrawer Clear()
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
