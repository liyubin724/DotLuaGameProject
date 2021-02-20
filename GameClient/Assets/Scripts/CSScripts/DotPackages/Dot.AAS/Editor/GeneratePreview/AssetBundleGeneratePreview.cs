using DotEditor.GUIExt;
using DotEditor.GUIExt.DataGrid;
using DotEditor.GUIExt.IMGUI;
using DotEditor.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS
{
    public class AssetBundleGeneratePreview : EditorWindow
    {
        [MenuItem("Game/AAS/Generate Preview")]
        public static void ShowWin()
        {
            AssetBundleGeneratePreview win = EditorWindow.GetWindow<AssetBundleGeneratePreview>();
            win.titleContent = new GUIContent("Generate Preview");
            win.Show();
        }

        private AssetBundleGenerateConfig[] generateConfigs = null;

        private ToolbarDrawer toolbarDrawer;
        private EasyListView bundleListView;
        private DragLineDrawer dragLineDrawer = null;
        private AssetBundleGridView assetGridView = null;

        private List<string> bundleNames = new List<string>();
        private Dictionary<string, List<AssetBundleBuildData>> bundleDataDic = new Dictionary<string, List<AssetBundleBuildData>>();

        private void SetGenerateConfigs(AssetBundleGenerateConfig[] configs)
        {
            generateConfigs = configs;
            RefreshGenerateConfigs();
        }

        private void SetSelectedBundleName(string bundleName)
        {
            if(!string.IsNullOrEmpty(bundleName)&& bundleDataDic.TryGetValue(bundleName, out var list))
            {
                assetGridView.SetDatas(list);
            }else
            {
                assetGridView.SetDatas(new List<AssetBundleBuildData>());
            }
        }

        private void RefreshGenerateConfigs()
        {
            bundleNames.Clear();
            bundleDataDic.Clear();
            SetSelectedBundleName(null);

            AssetBundleGenerateConfig[] targetConfigs = generateConfigs;
            if(targetConfigs == null)
            {
                targetConfigs = AssetDatabaseUtility.FindInstances<AssetBundleGenerateConfig>();
            }

            if(targetConfigs!=null)
            {
                foreach(var config in targetConfigs)
                {
                    AssetBundleBuildData[] datas = config.GetDatas();
                    if(datas!=null && datas.Length>0)
                    {
                        foreach(var data in datas)
                        {
                            if(!bundleDataDic.TryGetValue(data.bundle,out var list))
                            {
                                list = new List<AssetBundleBuildData>();
                                bundleDataDic.Add(data.bundle, list);
                            }
                            list.Add(data);
                        }
                    }
                }
            }

            bundleNames.AddRange(bundleDataDic.Keys);
        }

        private void OnEnable()
        {
            bundleListView = new EasyListView();
            bundleListView.HeaderContent = "Bundle Names";
            bundleListView.OnSelectedChange = (selected) =>
            {
                SetSelectedBundleName((string)selected);
            };
            assetGridView = new AssetBundleGridView();

            RefreshGenerateConfigs();

            foreach(var bName in bundleNames)
            {
                bundleListView.AddItem(bName, bName);
            }
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
                                RefreshGenerateConfigs();
                            }
                        }
                    ),
                    RightDrawable = new HorizontalLayoutDrawer
                    (
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.settingContent,
                            OnClicked = () =>
                            {

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
            Rect gridViewRect = new Rect(dragLineDrawer.MaxX, contentRect.y, contentRect.width - dragLineDrawer.MaxX, contentRect.height);
            EGUI.DrawAreaLine(gridViewRect, Color.yellow);
            assetGridView.OnGUI(gridViewRect);
        }

        class Contents 
        {
            public static string refreshContent = "Refresh";
            public static string settingContent = "Setting";
        }
    }
}
