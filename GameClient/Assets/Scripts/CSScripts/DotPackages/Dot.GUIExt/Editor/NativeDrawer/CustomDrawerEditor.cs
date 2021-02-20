using DotEngine.GUIExt.NativeDrawer;
using System;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
    public class CustomDrawerEditor : Editor
    {
        private ObjectDrawer objectDrawer = null;
        private CustomDrawerEditorAttribute attr = null;
        private void OnEnable()
        {
            Type targetType = target.GetType();
            var attrs = targetType.GetCustomAttributes(typeof(CustomDrawerEditorAttribute), true);
            if(attrs!=null && attrs.Length>0)
            {
                attr = attrs[0] as CustomDrawerEditorAttribute;
                if(attr.Enable)
                {
                    objectDrawer = new ObjectDrawer(target)
                    {
                        IsShowBox = attr.IsShowBox,
                        IsShowInherit = attr.IsShowInherit,
                        IsShowScroll = attr.IsShowScroll,
                        Header = attr.Header,
                    };
                }
            }
        }

        private void OnDisable()
        {
            objectDrawer = null;
        }

        public override void OnInspectorGUI()
        {
            if(objectDrawer!=null)
            {
                EGUILayout.DrawScript(target);
                EditorGUILayout.Space();

                float labelWidth = attr.LabelWidth;
                if(labelWidth<=0)
                {
                    labelWidth = EditorGUIUtility.labelWidth;
                }

                EGUI.BeginLabelWidth(labelWidth);
                {
                    objectDrawer.OnGUILayout();
                }
                EGUI.EndLableWidth();
            }else
            {
                base.OnInspectorGUI();
            }
        }
    }
}
