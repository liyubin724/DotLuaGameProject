using DotEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UI;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIRawImage))]
    public class UIRawImageEditor : RawImageEditor
    {
        SerializedProperty imageAddressProperty;
        SerializedProperty isSetNativeSizeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            imageAddressProperty = serializedObject.FindProperty("m_ImageAddress");
            isSetNativeSizeProperty = serializedObject.FindProperty("m_IsSetNativeSize");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(imageAddressProperty);
                EditorGUILayout.PropertyField(isSetNativeSizeProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
