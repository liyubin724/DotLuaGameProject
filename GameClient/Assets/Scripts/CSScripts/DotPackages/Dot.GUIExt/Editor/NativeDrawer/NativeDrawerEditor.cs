using DotEngine.GUIExt.NativeDrawer;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomEditor(typeof(ScriptableObject),true, isFallback = true)]
    public class NativeDrawerEditor : Editor
    {
        private ObjectDrawer objectDrawer = null;

        private void OnEnable()
        {
            Type targetType = target.GetType();
            var attrs = targetType.GetCustomAttributes(typeof(NativeDrawerEditorAttribute), false);
            if(attrs!=null && attrs.Length>0)
            {
                NativeDrawerEditorAttribute attr = attrs[0] as NativeDrawerEditorAttribute;
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
                objectDrawer.OnGUILayout();
            }else
            {
                base.OnInspectorGUI();
            }
        }
    }
}
