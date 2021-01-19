using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NInstanceDrawer : NLayoutDrawer
    {
        private bool isShowScroll = false;
        public bool IsShowScroll
        {
            get
            {
                return isShowScroll;
            }
            set
            {
                if(isShowScroll!=value)
                {
                    isShowScroll = value;
                    Refresh();
                }
            }
        }

        private bool isShowInherit = false;
        public bool IsShowInherit
        {
            get
            {
                return isShowInherit;
            }
            set
            {
                if(isShowInherit!=value)
                {
                    isShowInherit = value;
                    Refresh();
                }
            }
        }

        private bool isShowBox = false;
        public bool IsShowBox
        {
            get
            {
                return isShowBox;
            }
            set
            {
                if(isShowBox!=value)
                {
                    isShowBox = value;
                    Refresh();
                }
            }
        }
        private string header = null;
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                if(header!=value)
                {
                    header = value;
                    Refresh();
                }
            }
        }

        public SystemObject Target { get; private set; }
        protected List<NLayoutDrawer> childDrawers = new List<NLayoutDrawer>();

        private Vector2 scrollPos = Vector2.zero;
        private bool isNeedRefresh = true;

        protected NInstanceDrawer(SystemObject target)
        {
            Target = target;
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

        public void Refresh()
        {
            isNeedRefresh = true;
        }

        public override void OnGUILayout()
        {
            if(isNeedRefresh)
            {
                RefreshDrawers();
                isNeedRefresh = false;
            }

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
