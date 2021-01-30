using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class LineSettingWindow : EditorWindow
    {
        public static void ShowWin()
        {
            var win = GetWindow<LineSettingWindow>();
            win.titleContent = new GUIContent("Setting");
            win.Show();
        }

        void OnGUI()
        {
            LineSetting setting = LineSetting.Setting;
            if(setting == null)
            {
                return;
            }

            setting.TracklineHeight = EditorGUILayout.IntField("Trackline Height", setting.TracklineHeight);
            setting.TimeStep = EditorGUILayout.FloatField("Time Step", setting.TimeStep);
            setting.ZoomTimeStep = EditorGUILayout.FloatField("Zoom Time Step", setting.ZoomTimeStep);
            setting.WidthForSecond = EditorGUILayout.IntField("Width For Second", setting.WidthForSecond);

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.FloatField("Time Step Width", setting.TimeStepWidth);
                EditorGUILayout.Vector2Field("Scroll Pos", setting.ScrollPos);
                EditorGUILayout.IntField("Max Action Index", setting.MaxActionIndex);
                if(!string.IsNullOrEmpty(setting.CopiedActionData))
                {
                    EditorGUILayout.TextField("Copied Action", setting.CopiedActionData, EditorStyles.wordWrappedLabel, GUILayout.Height(60));
                }
            }
            EditorGUI.EndDisabledGroup();

        }
    }
}
