using DotEditor.GUIExtension;
using DotEditor.GUIExtension.TreeGUI;
using DotEditor.NativeDrawer;
using DotEditor.Utilities;
using DotEngine.Entity.Avatar;
using System.IO;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Entity.Avatar
{
    public class AvatarCreatorWindow : EditorWindow
    {
        [MenuItem("Game/Entity/Avatar Creator")]
        static void ShowWin()
        {
            var win = GetWindow<AvatarCreatorWindow>();
            win.titleContent = new GUIContent("Avatar Creator");
            win.Show();
        }

        private static int TOOLBAR_HEIGHT = 18;
        private static int DATA_LIST_WIDTH = 200;
        private static int LINE_THINKNESS = 1;

        private EGUIListView<string> dataListView;

        private AvatarCreatorData currentCreatorData = null;
        private NativeDrawerObject partOutputDataDrawer = null;
        private NativeDrawerObject skeletonCreatorDataDrawer = null;

        private AvatarPreviewer previewer = null;

        void OnEnable()
        {
            string[] assetPaths = AssetDatabaseUtility.FindAssets<AvatarCreatorData>();

            dataListView = new EGUIListView<string>
            {
                Header = "Data List",
                OnSelectedChange = OnListViewItemSelected,
                OnDrawItem = (rect, index) =>
                {
                    string assetPath = dataListView.GetItem(index);
                    EditorGUI.LabelField(rect, Path.GetFileNameWithoutExtension(assetPath), EGUIStyles.BoldLabelStyle);
                },
            };
            dataListView.AddItems(assetPaths);

            AvatarPartCreatorDataDrawer.CreatePartBtnClick = (data) =>
            {
                CreatePart(data);
            };
            AvatarPartCreatorDataDrawer.PreviewPartBtnClick = (data) =>
            {
                PreviewPart(data);
            };

            previewer = new AvatarPreviewer();
        }

        void OnDisable()
        {
            previewer.Dispose();

            AvatarPartCreatorDataDrawer.CreatePartBtnClick = null;
            AvatarPartCreatorDataDrawer.PreviewPartBtnClick = null;

            AssetDatabase.SaveAssets();
        }

        private void OnListViewItemSelected(int index)
        {
            currentCreatorData = null;
            skeletonCreatorDataDrawer = null;
            partOutputDataDrawer = null;

            string assetPath = dataListView.GetItem(index);
            if (!string.IsNullOrEmpty(assetPath))
            {
                currentCreatorData = AssetDatabase.LoadAssetAtPath<AvatarCreatorData>(assetPath);
                skeletonCreatorDataDrawer = new NativeDrawerObject(currentCreatorData.skeletonData)
                {
                    IsShowScroll = true,
                };
                partOutputDataDrawer = new NativeDrawerObject(currentCreatorData.skeletonPartData)
                {
                    IsShowScroll = true
                };
            }

            Repaint();
        }

        void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, position.height);

            Rect toolbarRect = new Rect(rect.x, rect.y, position.width, TOOLBAR_HEIGHT);
            EditorGUI.LabelField(toolbarRect, GUIContent.none, EditorStyles.toolbar);
            DrawToolbar(toolbarRect);

            Rect dataListRect = new Rect(rect.x + LINE_THINKNESS, rect.y + TOOLBAR_HEIGHT + LINE_THINKNESS,
                                                                DATA_LIST_WIDTH - LINE_THINKNESS * 2, rect.height - TOOLBAR_HEIGHT - LINE_THINKNESS * 2);
            dataListView.OnGUI(dataListRect);

            Rect skeletonRect = new Rect(dataListRect.x + dataListRect.width, dataListRect.y, (rect.width - dataListRect.width) * 0.4f, dataListRect.height);
            EGUI.DrawAreaLine(skeletonRect, Color.black);
            DrawSkeleton(skeletonRect);

            Rect partRect = new Rect(skeletonRect.x + skeletonRect.width, dataListRect.y, (rect.width - dataListRect.width) * 0.6f, dataListRect.height);
            EGUI.DrawAreaLine(partRect, Color.black);
            DrawParts(partRect);

            if (GUI.changed && currentCreatorData != null)
            {
                EditorUtility.SetDirty(currentCreatorData);
            }
        }

        private void DrawToolbar(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                GUILayout.BeginHorizontal();
                {
                    if (EGUILayout.ToolbarButton("New"))
                    {
                        var newData = EGUIUtility.CreateAsset<AvatarCreatorData>();
                        if (newData != null)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(newData);
                            dataListView.AddItem(assetPath);

                            dataListView.SetSelection(dataListView.GetCount() - 1);
                        }
                    }
                    if (EGUILayout.ToolbarButton("Delete"))
                    {
                        if (currentCreatorData != null)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(currentCreatorData);
                            dataListView.RemoveItem(assetPath);

                            DeleteCreatorData(currentCreatorData);

                            dataListView.SetSelection(-1);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawSkeleton(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();
                {
                    EGUILayout.DrawBoxHeader("Skeleton Data", EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
                    if (currentCreatorData != null && skeletonCreatorDataDrawer != null)
                    {
                        skeletonCreatorDataDrawer.OnGUILayout();

                        AvatarSkeletonCreatorData skeletonCreatorData = currentCreatorData.skeletonData;

                        string targetPrefabPath = skeletonCreatorData.GetSkeletonPrefabPath();
                        GameObject targetPrefab = null;
                        if (!string.IsNullOrEmpty(targetPrefabPath))
                        {
                            targetPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(targetPrefabPath);
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        EditorGUILayout.ObjectField("Output", targetPrefab, typeof(GameObject), false);

                        EditorGUILayout.Space();

                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Create Skeleton"))
                        {
                            GameObject skeletonPrefab = AvatarCreatorUtil.CreateSkeleton(skeletonCreatorData);
                            if (skeletonPrefab == null)
                            {
                                EditorUtility.DisplayDialog("Error", "Create Failed.\n Please view the details from the console!!!", "OK");
                            }
                            else
                            {
                                SelectionUtility.PingObject(skeletonPrefab);
                            }
                        }

                        if (GUILayout.Button("Preview Skeleton"))
                        {
                            PreviewSkeleton();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private void DrawParts(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();
                {
                    EGUILayout.DrawBoxHeader("Part Data", EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
                    if (currentCreatorData != null && partOutputDataDrawer != null)
                    {
                        partOutputDataDrawer.OnGUILayout();


                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Create Parts"))
                        {
                            AvatarSkeletonPartCreatorData partOutputData = currentCreatorData.skeletonPartData;
                            foreach (var data in partOutputData.partDatas)
                            {
                                if (!CreatePart(data))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private bool CreatePart(AvatarPartCreatorData data)
        {
            var partData = AvatarCreatorUtil.CreatePart(currentCreatorData.skeletonPartData.outputFolder, data);
            if (partData == null)
            {
                EditorUtility.DisplayDialog("Error", "Create Failed.\n Please view the details from the console!!!", "OK");
                return false;
            }
            else
            {
                SelectionUtility.PingObject(partData);
                return true;
            }
        }

        private void PreviewSkeleton()
        {
            AvatarSkeletonCreatorData skeletonCreatorData = currentCreatorData.skeletonData;
            string skeletonAssetPath = skeletonCreatorData.GetSkeletonPrefabPath();
            if (AssetDatabaseUtility.IsAssetAtPath<GameObject>(skeletonAssetPath))
            {
                previewer.LoadSkeleton(skeletonAssetPath);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", $"The prefab is not found in \"{skeletonAssetPath}\"", "OK");
            }
        }

        private void PreviewPart(AvatarPartCreatorData data)
        {
            if (!previewer.HasSkeleton())
            {
                EditorUtility.DisplayDialog("Error", $"Please preview the skeleton at first", "OK");
                return;
            }

            string assetPath = data.GetPartAssetPath(currentCreatorData.skeletonPartData.outputFolder);
            if (AssetDatabaseUtility.IsAssetAtPath<AvatarPartData>(assetPath))
            {
                previewer.AddPart(assetPath);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", $"The asset is not found in \"{assetPath}\"", "OK");
            }
        }

        private void DeleteCreatorData(AvatarCreatorData data)
        {
            string skeletonAssetPath = data.skeletonData.GetSkeletonPrefabPath();
            if (AssetDatabase.LoadAssetAtPath<UnityObject>(skeletonAssetPath) != null)
            {
                AssetDatabase.DeleteAsset(skeletonAssetPath);
            }

            AvatarSkeletonPartCreatorData partOutputData = data.skeletonPartData;

            foreach (var partData in partOutputData.partDatas)
            {
                string partAssetPath = partData.GetPartAssetPath(partOutputData.outputFolder);
                AvatarPartData avatarPartData = AssetDatabase.LoadAssetAtPath<AvatarPartData>(partAssetPath);
                if (avatarPartData != null)
                {
                    foreach (var avatarRendererPartData in avatarPartData.rendererParts)
                    {
                        if (avatarRendererPartData.mesh != null)
                        {
                            string meshAssetPath = AssetDatabase.GetAssetPath(avatarRendererPartData.mesh);
                            if (Path.GetExtension(meshAssetPath).ToLower() == ".asset" && Path.GetFileNameWithoutExtension(meshAssetPath).EndsWith("_mesh"))
                            {
                                AssetDatabase.DeleteAsset(meshAssetPath);
                            }
                        }
                    }

                    AssetDatabase.DeleteAsset(partAssetPath);
                }
            }

            string assetPath = AssetDatabase.GetAssetPath(currentCreatorData);
            AssetDatabase.DeleteAsset(assetPath);

        }
    }
}
