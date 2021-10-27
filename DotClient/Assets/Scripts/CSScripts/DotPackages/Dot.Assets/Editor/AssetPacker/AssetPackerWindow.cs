using DotEditor.GUIExtension;
using DotEditor.TreeGUI;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Assets.Packer
{
    public enum RunMode
    {
        AssetDatabase,
        AssetBundle,
    }

    public enum SearchCategory
    {
        All = 0,
        Group = 1,
        Bundle = 2,
        AssetAddress = 3,
        AssetPath = 4,
        AssetLabels = 5,
    }

    public class AssetPackerWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Asset Packer Window", priority = 10)]
        public static void ShowWin()
        {
            AssetPackerWindow win = EditorWindow.GetWindow<AssetPackerWindow>();
            win.titleContent = new GUIContent("Asset Packer");
            win.minSize = new Vector2(Styles.PackOperationWidth * 2, Styles.PackOperationWidth);
            win.Show();
        }
        private static readonly string ASSET_BUNDLE_SYMBOL = "ASSET_BUNDLE";

        private RunMode runMode = RunMode.AssetDatabase;

        private bool isExpandAll = false;

        private ToolbarSearchField searchField = null;
        public string[] SearchCategories = new string[]
        {
            "All",
            "Group",
            "Bundle",
            "Asset Address",
            "Asset Path",
            "Asset Labels",
        };
        private SearchCategory searchCategory = SearchCategory.All;
        private string searchText = "";

        private AssetPackerTreeView assetPackerTreeView;
        private TreeViewState assetPackerTreeViewState;

        private PackerData packerData = null;
        public PackerData GetData() => packerData;
        public PackerGroupData GetGroupData(int groupIndex)
        {
            return packerData.groupDatas[groupIndex];
        }
        public PackerBundleData GetBundleData(int groupIndex, int bundleIndex)
        {
            return GetGroupData(groupIndex).bundleDatas[bundleIndex];
        }
        public PackerAssetData GetAssetData(int groupIndex,int bundleIndex,int assetIndex)
        {
            return GetBundleData(groupIndex, bundleIndex).assetDatas[assetIndex];
        }

        //private Dictionary<string, List<PackerBundleData>> addressRepeatDataDic = new Dictionary<string, List<PackerBundleData>>();
        private void OnEnable()
        {
            packerData = AssetPackerUtil.GetPackerData();

            //foreach(var group in assetPackerConfig.groupDatas)
            //{
            //    foreach(var data in group.assetFiles)
            //    {
            //        if(!addressRepeatDataDic.TryGetValue(data.Address,out List<PackerBundleData> dataList))
            //        {
            //            dataList = new List<PackerBundleData>();
            //            addressRepeatDataDic.Add(data.Address, dataList);
            //        }
            //        dataList.Add(data);
            //    }
            //}

            if (PlayerSettingsUtility.HasScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL))
            {
                runMode = RunMode.AssetBundle;
            }
        }

        public bool IsAddressRepeated(string address,out List<PackerBundleData> datas)
        {
            //if(addressRepeatDataDic.TryGetValue(address,out List<PackerBundleData> list))
            //{
            //    if(list.Count>1)
            //    {
            //        datas = list;
            //        return true;
            //    }
            //}

            datas = null;
            return false;
        }

        public bool IsGroupAddressRepeated(PackerGroupData group)
        {
            //foreach(var data in group.assetFiles)
            //{
            //    if(IsAddressRepeated(data.Address, out List <PackerBundleData> list))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        Rect lastRect = Rect.zero;
        Rect treeRect = Rect.zero;
        Rect operationRect = Rect.zero;
        private void OnGUI()
        {
            DrawToolbar();

            EditorGUILayout.LabelField("Asset Packer Group", EGUIStyles.MiddleCenterLabel, GUILayout.ExpandWidth(true));

            if (assetPackerTreeView == null)
            {
                InitTreeView();
                EditorApplication.delayCall += () =>
                {
                    SetTreeModel();
                };
            }
            GUILayout.Label(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if(Event.current.type == EventType.Repaint)
            {
                lastRect = GUILayoutUtility.GetLastRect();
                
                treeRect = lastRect;
                treeRect.width -= Styles.PackOperationWidth;

                operationRect = new Rect(treeRect.x + treeRect.width, treeRect.y, Styles.PackOperationWidth, treeRect.height);
            }
            assetPackerTreeView?.OnGUI(treeRect);
            GUILayout.BeginArea(operationRect);
            {
                DrawPackOperation();
            }
            GUILayout.EndArea();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button(isExpandAll ? "Collapse All" : "Expand All", EditorStyles.toolbarButton, GUILayout.Width(Styles.ToolbarBtnWidth)))
                {
                    isExpandAll = !isExpandAll;
                    if (isExpandAll)
                    {
                        assetPackerTreeView.ExpandAll();
                    }
                    else
                    {
                        assetPackerTreeView.CollapseAll();
                    }
                }
                Rect menuRect = GUILayoutUtility.GetRect(Contents.RunModeContent, EditorStyles.toolbarButton);
                if (EditorGUI.DropdownButton(menuRect, Contents.RunModeContent,FocusType.Passive,EditorStyles.toolbarButton))
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent(RunMode.AssetDatabase.ToString()), runMode == RunMode.AssetDatabase, () =>
                     {
                         PlayerSettingsUtility.RemoveScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL);
                     });
                    menu.AddItem(new GUIContent(RunMode.AssetBundle.ToString()), runMode == RunMode.AssetBundle, () =>
                    {
                        PlayerSettingsUtility.AddScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL);
                    });
                    menu.DropDown(menuRect);
                }

                GUILayout.FlexibleSpace();

                if(searchField == null)
                {
                    searchField = new ToolbarSearchField((text) =>
                    {
                        if(searchText!=text)
                        {
                            searchText = text;
                            SetTreeModel();
                        }
                    }, (category) =>
                    {
                        int newIndex = Array.IndexOf(SearchCategories, category);
                        if((int)searchCategory!=newIndex)
                        {
                            searchCategory = (SearchCategory)newIndex;
                            SetTreeModel();
                        }
                    });

                    searchField.Categories = SearchCategories;
                    searchField.CategoryIndex = 0;
                }
                searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        private BundleBuildData bundleBuildConfig = null;
        private void DrawPackOperation()
        {
            if(bundleBuildConfig == null)
            {
                bundleBuildConfig = AssetPackerUtil.ReadBuildData();
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.LabelField(Contents.BuildTitleContent, EGUIStyles.MiddleCenterLabel);
                EditorGUI.BeginChangeCheck();
                {
                    EGUI.BeginLabelWidth(120);
                    {
                        bundleBuildConfig.OutputDir = EGUILayout.DrawDiskFolderSelection(Contents.BuildOutputDirStr, bundleBuildConfig.OutputDir);
                        bundleBuildConfig.CleanupBeforeBuild = EditorGUILayout.Toggle(Contents.BuildCleanup, bundleBuildConfig.CleanupBeforeBuild);
                        bundleBuildConfig.PathFormat = (BundlePathFormatType)EditorGUILayout.EnumPopup(Contents.BuildPathFormatContent, bundleBuildConfig.PathFormat);
                        EditorGUILayout.Space();
                        bundleBuildConfig.IsForceRebuild = EditorGUILayout.Toggle(Contents.ForceRebuild, bundleBuildConfig.IsForceRebuild);
                        bundleBuildConfig.Target = (ValidBuildTarget)EditorGUILayout.EnumPopup(Contents.BuildTargetContent, bundleBuildConfig.Target);
                        bundleBuildConfig.Compression = (CompressOption)EditorGUILayout.EnumPopup(Contents.BuildCompressionContent, bundleBuildConfig.Compression);
                    }
                    EGUI.EndLableWidth();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    AssetPackerUtil.WriteBuildData(bundleBuildConfig);
                }

                GUILayout.FlexibleSpace();

                EGUI.BeginGUIBackgroundColor(Color.red);
                {
                    if (GUILayout.Button(Contents.OperationAutoBuildContent, GUILayout.Height(80), GUILayout.ExpandWidth(true)))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            AssetPackerUtil.BuildAssetBundles(packerData, bundleBuildConfig);
                        };
                    }
                }
                EGUI.EndGUIBackgroundColor();
            }
            EditorGUILayout.EndVertical();
        }

        private void InitTreeView()
        {
            assetPackerTreeViewState = new TreeViewState();
            TreeModel<TreeElementWithData<AssetPackerTreeData>> model = new TreeModel<TreeElementWithData<AssetPackerTreeData>>(
               new List<TreeElementWithData<AssetPackerTreeData>>()
               {
                    new TreeElementWithData<AssetPackerTreeData>(AssetPackerTreeData.Root,"",-1,-1),
               });
            assetPackerTreeView = new AssetPackerTreeView(assetPackerTreeViewState, model);
            assetPackerTreeView.Window = this;
        }

        private bool FilterGroupData(PackerGroupData groupData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }
            if (searchCategory == SearchCategory.All || searchCategory == SearchCategory.Group)
            {
                if (!string.IsNullOrEmpty(groupData.Name) && 
                    groupData.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }

                if (searchCategory == SearchCategory.Group)
                {
                    return false;
                }
            }

            foreach (var bundleData in groupData.bundleDatas)
            {
                if(FilterBundleData(bundleData))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FilterBundleData(PackerBundleData bundleData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            if(searchCategory == SearchCategory.All || searchCategory == SearchCategory.Bundle)
            {
                if(bundleData.Path.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }

                if(searchCategory == SearchCategory.Bundle)
                {
                    return false;
                }
            }

            foreach (var assetData in bundleData.assetDatas)
            {
                if(FilterAddressData(assetData))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FilterAddressData(PackerAssetData assetData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            if (searchCategory == SearchCategory.All || searchCategory == SearchCategory.AssetAddress)
            {
                if (!string.IsNullOrEmpty(assetData.Address) && 
                    assetData.Address.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            if (searchCategory == SearchCategory.All || searchCategory == SearchCategory.AssetPath)
            {
                if (!string.IsNullOrEmpty(assetData.Path) &&
                    assetData.Path.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            if (searchCategory == SearchCategory.All || searchCategory == SearchCategory.AssetLabels)
            {
                string label = string.Join(",", assetData.Labels);
                if (!string.IsNullOrEmpty(label) &&
                    label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetTreeModel()
        {
            TreeModel<TreeElementWithData<AssetPackerTreeData>> treeModel = assetPackerTreeView.treeModel;
            TreeElementWithData<AssetPackerTreeData> treeModelRoot = treeModel.root;
            treeModelRoot.children?.Clear();

            int id = 1;
            for(int i = 0;i<packerData.groupDatas.Count;++i)
            {
                var groupData = packerData.groupDatas[i];
                if(FilterGroupData(groupData))
                {
                    AssetPackerTreeData groupTreeData = new AssetPackerTreeData();
                    groupTreeData.GroupIndex = i;

                    TreeElementWithData<AssetPackerTreeData> groupElementData = new TreeElementWithData<AssetPackerTreeData>(groupTreeData, "", 0, id);
                    treeModel.AddElement(groupElementData, treeModelRoot, treeModelRoot.hasChildren ? treeModelRoot.children.Count : 0);
                    id++;

                    for(int j = 0;j<groupData.bundleDatas.Count;j++)
                    {
                        PackerBundleData bundleData = groupData.bundleDatas[j];
                        if(FilterBundleData(bundleData))
                        {
                            AssetPackerTreeData bundleTreeData = new AssetPackerTreeData();
                            bundleTreeData.GroupIndex = i;
                            bundleTreeData.BundleIndex = j;

                            TreeElementWithData<AssetPackerTreeData> bundleElementData = new TreeElementWithData<AssetPackerTreeData>(bundleTreeData, "", 0, id);
                            treeModel.AddElement(bundleElementData, groupElementData, groupElementData.hasChildren ? groupElementData.children.Count : 0);
                            id++;

                            for(int k = 0;k<bundleData.assetDatas.Count;k++)
                            {
                                PackerAssetData assetData = bundleData.assetDatas[k];
                                if(FilterAddressData(assetData))
                                {
                                    AssetPackerTreeData assetTreeData = new AssetPackerTreeData();
                                    assetTreeData.GroupIndex = i;
                                    assetTreeData.BundleIndex = j;
                                    assetTreeData.AssetIndex = k;

                                    TreeElementWithData<AssetPackerTreeData> assetElementData = new TreeElementWithData<AssetPackerTreeData>(assetTreeData, "", 0, id);
                                    treeModel.AddElement(assetElementData, bundleElementData, bundleElementData.hasChildren ? bundleElementData.children.Count : 0);
                                    id++;

                                }
                            }
                        }
                    }
                }
            }

        }

        static class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Asset Packer");
            internal static GUIContent RunModeContent = new GUIContent("Run Mode");
            internal static GUIContent BundlePathFormatContent = new GUIContent("Bundle Path Format");

            internal static GUIContent BuildTitleContent = new GUIContent("Build Config");

            internal static string BuildOutputDirStr = "Output Dir";
            internal static GUIContent BuildPathFormatContent = new GUIContent("Path Format");
            internal static GUIContent BuildCleanup = new GUIContent("Is Cleanup");
            internal static GUIContent ForceRebuild = new GUIContent("Is Force Rebuild");
            internal static GUIContent BuildTargetContent = new GUIContent("Build Target");
            internal static GUIContent BuildCompressionContent = new GUIContent("Compression");

            internal static GUIContent OperationTitleContent = new GUIContent("Operation");

            internal static GUIContent OperationAutoBuildContent = new GUIContent("Pack Bundle");
        }

        static class Styles
        {
            internal static int PackOperationWidth = 300;
            internal static int PackOperationMaxWidth = 300;
            internal static int PackOperationMinWidth = 200;
            internal static int ToolbarBtnWidth = 120; 

        }
    }
}
