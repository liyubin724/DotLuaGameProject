using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NInstanceDrawer : NLayoutDrawer
    {
        public bool IsShowScroll { get; set; } = true;
        public bool IsShowInherit { get; set; } = true;
        public bool IsShowBox { get; set; } = false;
        public string Header { get; set; } = null;

        public SystemObject Target { get; private set; }
        protected List<NLayoutDrawer> childDrawers = new List<NLayoutDrawer>();

        private Vector2 scrollPos = Vector2.zero;
        protected NInstanceDrawer(SystemObject target)
        {
            Target = target;

            RefreshDrawers();
        }

        protected void RefreshDrawers()
        {
            childDrawers.Clear();

            if (!string.IsNullOrEmpty(Header))
            {
                childDrawers.Add(new NHeadDrawer() { Header = Header });
            }

            if(Target!=null)
            {
                InitDrawers();
            }
        }

        protected abstract void InitDrawers();

        public override void OnGUILayout()
        {
            if (IsShowScroll)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, IsShowBox ? EditorStyles.helpBox : GUIStyle.none);
            }

            EditorGUILayout.BeginVertical((IsShowBox && !IsShowScroll) ? EditorStyles.helpBox : GUIStyle.none);
            {
                if(Target == null)
                {
                    DrawNull();
                }else
                {
                    DrawInstance();
                }
            }
            EditorGUILayout.EndVertical();

            if (IsShowScroll)
            {
                EditorGUILayout.EndScrollView();
            }

            if (Target != null && typeof(UnityObject).IsAssignableFrom(Target.GetType()))
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty((UnityObject)Target);
                }
            }
        }

        protected virtual void DrawNull()
        {
            EditorGUILayout.LabelField("");
        }

        protected virtual void DrawInstance()
        {
            foreach (var drawer in childDrawers)
            {
                drawer.OnGUILayout();
            }
        }
    }
}
