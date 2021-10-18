using DotEditor.GUIExt;
using DotEngine.NativeDrawer;
using System;
using UnityEditor;

namespace DotEditor.NativeDrawer
{
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
    public class CustomDrawerEditor : Editor
    {
        private CustomDrawerEditorAttribute attr;
        private DrawerObject drawerObject = null;   

        void OnEnable()
        {
            Type targetType = target.GetType();
            var attrs = targetType.GetCustomAttributes(typeof(CustomDrawerEditorAttribute), true);
            if(attrs!=null && attrs.Length>0)
            {
                attr = (CustomDrawerEditorAttribute)attrs[0];
            }
            if(attr!=null)
            {
                drawerObject = new DrawerObject(target)
                {
                    IsShowScroll = attr.IsShowScroll,
                    IsShowInherit = attr.IsShowInherit,
                };
            }
        }

        public override void OnInspectorGUI()
        {
            if(attr!=null)
            {
                EGUILayout.DrawScript(target);
                DrawerSetting.OnDrawSetting();

                EGUI.BeginLabelWidth(attr.LabelWidth);
                {
                    drawerObject.OnGUILayout();
                }
                EGUI.EndLableWidth();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}
