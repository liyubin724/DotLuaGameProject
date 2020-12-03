using DotEngine.BMF;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Fonts
{
    [CustomEditor(typeof(BitmapFont))]
    public class BitmapFontEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("");
            if(GUILayout.Button("Editor"))
            {

            }
            EditorGUI.BeginDisabledGroup(true);
            {
                base.OnInspectorGUI();
            }
            EditorGUI.EndDisabledGroup();
        }

    }
}
