using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.AssetPacker
{
    public class AssetAddressRepeatPopupContent : PopupWindowContent
    {
        public AssetPackerAddressData[] RepeatAddressDatas { get; set; }
        private Vector2 scrollPos = Vector2.zero;
        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.LabelField($"Repeat Address({RepeatAddressDatas[0].assetAddress})", EGUIStyles.MiddleCenterLabel);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
                {
                    for(int i =0;i<RepeatAddressDatas.Length;i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("" + i,EGUIStyles.MiddleLeftLabelStyle, GUILayout.Width(20),GUILayout.Height(40));
                            EditorGUILayout.BeginVertical();
                            {
                                EGUI.BeginLabelWidth(60);
                                {
                                    EditorGUILayout.TextField("Path : ", RepeatAddressDatas[i].assetPath);

                                    UnityObject uObj = AssetDatabase.LoadAssetAtPath<UnityObject>(RepeatAddressDatas[i].assetPath);
                                    EditorGUILayout.ObjectField("Target:",uObj, typeof(UnityObject), false);
                                }
                                EGUI.EndLableWidth();
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();

                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        public override Vector2 GetWindowSize()
        {
            return new Vector2(400, 300);
        }
    }
}
