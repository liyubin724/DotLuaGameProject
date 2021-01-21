using DotEditor.GUIExt;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    public class CheckerSettingPopupContent : GUIExt.Windows.PopupWindowContent
    {
        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();
                {
                    EGUILayout.DrawBoxHeader("Setting", EGUIStyles.BoxedHeaderCenterStyle);
                    EditorGUILayout.Space();

                    CheckerSetting setting = CheckerUtility.ReadSetting();
                    setting.isFolderAsAsset = EditorGUILayout.Toggle("Is Asset For Folder", setting.isFolderAsAsset);

                    GUILayout.FlexibleSpace();


                    if(GUILayout.Button("Close"))
                    {
                        Window.CloseWindow();
                    }

                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
    }
}
