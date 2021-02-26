using DotEditor.GUIExt;
using DotEditor.GUIExt.DataGrid;
using DotEditor.GUIExt.IMGUI;
using DotEditor.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.AAS.Packer
{
    public class BundleGeneratePreview : EditorWindow
    {
        [MenuItem("Game/Asset/AAS/Packer")]
        public static void ShowWin()
        {
            BundleGeneratePreview win = GetWindow<BundleGeneratePreview>();
            win.titleContent = new GUIContent("Generate Preview");
            win.Show();
        }

        public static void ShowWin(GenerateBundleConfig[] configs)
        {
            BundleGeneratePreview win = GetWindow<BundleGeneratePreview>();
            win.titleContent = new GUIContent("Generate Preview");
            win.generateConfigs = configs;
            win.Show();
        }

        private GenerateBundleConfig[] generateConfigs = null;

        private ToolbarDrawer toolbarDrawer;
        private EasyListView bundleListView;
        private DragLineDrawer dragLineDrawer = null;
        private AssetGridView assetGridView = null;

        private List<string> bundleNames = new List<string>();
        private Dictionary<string, List<AssetGridViewData>> assetInBundleDataDic = new Dictionary<string, List<AssetGridViewData>>();

        private bool usedMD5AsBundleName = false;
        private bool isForceRebuild = false;

        private void OnEnable()
        {
            RefreshDatas();
        }

        private string selectedBundleName = string.Empty;
        private void RefreshAssetGridView()
        {
            if (assetGridView != null)
            {
                if (!string.IsNullOrEmpty(selectedBundleName) && assetInBundleDataDic.TryGetValue(selectedBundleName, out var list))
                {
                    assetGridView.SetDatas(list);
                }
                else
                {
                    assetGridView.SetDatas(new List<AssetGridViewData>());
                }
            }
        }

        private void RefreshBundleListView()
        {
            if (bundleListView != null)
            {
                bundleListView.Clear();

                foreach (var bName in bundleNames)
                {
                    bundleListView.AddItem(bName, bName);
                }
                bundleListView.SetSelectedItem(selectedBundleName, TreeViewSelectionOptions.None);
            }
        }

        private void RefreshDatas()
        {
            bundleNames.Clear();
            assetInBundleDataDic.Clear();

            GenerateBundleConfig[] targetConfigs = generateConfigs;
            if (targetConfigs == null)
            {
                targetConfigs = AssetDatabaseUtility.FindInstances<GenerateBundleConfig>();
            }

            if (targetConfigs != null)
            {
                foreach (var config in targetConfigs)
                {
                    GeneratedAssetData[] datas = config.GetDatas();
                    if (datas != null && datas.Length > 0)
                    {
                        foreach (var data in datas)
                        {
                            if (!assetInBundleDataDic.TryGetValue(data.bundle, out var list))
                            {
                                list = new List<AssetGridViewData>();
                                assetInBundleDataDic.Add(data.bundle, list);
                            }
                            list.Add(new AssetGridViewData()
                            {
                                Userdata = data,
                                configAssetPath = AssetDatabase.GetAssetPath(config)
                            });
                        }
                    }
                }
            }

            bundleNames.AddRange(assetInBundleDataDic.Keys);
            if(!string.IsNullOrEmpty(selectedBundleName) && bundleNames.IndexOf(selectedBundleName)<0)
            {
                selectedBundleName = null;
            }

            RefreshBundleListView();
            RefreshAssetGridView();
        }

        private Rect contentRect;
        private void OnGUI()
        {
            DrawToolbar();
            DrawDragline();

            Rect cRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if (cRect != contentRect && cRect.width > 10 && cRect.height > 10)
            {
                contentRect = cRect;
                dragLineDrawer.Position = new Rect(contentRect.x + 300, contentRect.y, 6, contentRect.height);
            }

            DrawBundleListView();
            DrawAssetGridView();
        }

        private void DrawToolbar()
        {
            if (toolbarDrawer == null)
            {
                toolbarDrawer = new ToolbarDrawer()
                {
                    LeftDrawable = new HorizontalLayoutDrawer
                    (
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.refreshContent,
                            OnClicked = () =>
                            {
                                RefreshDatas();
                                Repaint();
                            }
                        }
                    ),
                    RightDrawable = new HorizontalLayoutDrawer
                    (
                        new ToolbarToggleDrawer()
                        {
                            Text = Contents.isForceRebuild,
                            Value = isForceRebuild,
                            OnValueChanged = (isSelected) =>
                            {
                                isForceRebuild = isSelected;
                            }
                        },
                        new ToolbarToggleDrawer()
                        {
                            Text = Contents.usedMD5AsBundleName,
                            Value = usedMD5AsBundleName,
                            OnValueChanged = (isSelected) =>
                            {
                                usedMD5AsBundleName = isSelected;
                            }
                        },
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.buildContent,
                            OnClicked = () =>
                            {
                                GenericMenu menu = new GenericMenu();
                                menu.AddItem(Contents.buildWinContent, false, () =>
                                {
                                    BuildBundle(BuildTarget.StandaloneWindows);
                                });
                                menu.AddItem(Contents.buildAndroidContent, false, () =>
                                {
                                    BuildBundle(BuildTarget.Android);
                                });
                                menu.AddItem(Contents.buildIOSContent, false, () =>
                                {
                                    BuildBundle(BuildTarget.iOS);
                                });
                                menu.ShowAsContext();
                            }
                        }
                    ),
                };
            }
            toolbarDrawer.OnGUILayout();
        }

        private void DrawDragline()
        {
            if(dragLineDrawer == null)
            {
                dragLineDrawer = new DragLineDrawer(this, DragLineDirection.Vertical);
            }
            dragLineDrawer.LowLimitXY = 200;
            dragLineDrawer.UpperLimitXY = contentRect.width * 0.8f;
            dragLineDrawer.OnGUILayout();
        }

        private void DrawBundleListView()
        {
            if(bundleListView == null)
            {
                bundleListView = new EasyListView();
                bundleListView.HeaderContent = "Bundle Names";
                bundleListView.OnSelectedChange = (selected) =>
                {
                    selectedBundleName = (string)selected;
                    RefreshAssetGridView();
                };

                RefreshBundleListView();
            }

            Rect listViewRect = new Rect(contentRect.x, contentRect.y, dragLineDrawer.MinX - contentRect.x, contentRect.height);
            EGUI.DrawAreaLine(listViewRect, Color.blue);
            GUILayout.BeginArea(listViewRect);
            {
                bundleListView.OnGUILayout();
            }
            GUILayout.EndArea();
        }

        private void DrawAssetGridView()
        {
            if(assetGridView==null)
            {
                assetGridView = new AssetGridView();
                RefreshAssetGridView();
            }

            Rect gridViewRect = new Rect(dragLineDrawer.MaxX, contentRect.y, contentRect.width - dragLineDrawer.MaxX, contentRect.height);
            EGUI.DrawAreaLine(gridViewRect, Color.yellow);
            assetGridView.OnGUI(gridViewRect);
        }

        private void BuildBundle(BuildTarget buildTarget)
        {
            BundlePackerUtility.PackBundle(buildTarget, usedMD5AsBundleName, isForceRebuild);
        }

        class Contents 
        {
            public static string refreshContent = "Refresh";
            public static string buildContent = "Build";
            public static string isForceRebuild = "Force Rebuild";
            public static string usedMD5AsBundleName = "Used MD5";

            public static GUIContent buildWinContent = new GUIContent("Window"); 
            public static GUIContent buildAndroidContent = new GUIContent("Android");
            public static GUIContent buildIOSContent = new GUIContent("iOS");

        }
    }
}
