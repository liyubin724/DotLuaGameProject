using DotEditor.GUIExt;
using UnityEditor;
using UnityEngine;

using PopupWindow = DotEditor.GUIExt.Windows.PopupWindow;
using PopupContent = DotEditor.GUIExt.Windows.PopupWindowContent;

namespace DotEditor.AssetChecker
{
    public class CheckerSettingPopupContent : PopupContent
    {
        public static void ShowWin(Vector2 position)
        {
            PopupWindow.ShowWin(new Rect(position.x- 200,position.y+5,400,300), new CheckerSettingPopupContent(), true, false);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();
                {
                    EGUILayout.DrawBoxHeader("Setting", EGUIStyles.BoxedHeaderCenterStyle,GUILayout.ExpandWidth(true));
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
