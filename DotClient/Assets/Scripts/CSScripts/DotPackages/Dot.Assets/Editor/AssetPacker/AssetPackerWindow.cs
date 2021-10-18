using DotEditor.GUIExtension;
using DotEditor.TreeGUI;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Asset.AssetPacker
{
    public enum RunMode
    {
        AssetDatabase,
        AssetBundle,
    }

    public class AssetPackerWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Packer Window",priority =10)]
        public static void ShowWin()
        {
            AssetPackerWindow win = EditorWindow.GetWindow<AssetPackerWindow>();
            win.titleContent = new GUIContent("Bundle Packer");
            win.Show();
        }
        private static readonly string ASSET_BUNDLE_SYMBOL = "ASSET_BUNDLE";

        private RunMode runMode = RunMode.AssetDatabase;
        
        private bool isExpandAll = false;

        private ToolbarSearchField searchField = null;
        public string[] SearchCategories = new string[]
        {
            "All",
            "Address",
            "Path",
            "Bundle",
            "Labels",
        };
        private int searchCategoryIndex = 0;
        private string searchText = "";

        private AssetPackerTreeView assetPackerTreeView;
        private TreeViewState assetPackerTreeViewState;

        private AssetPackerConfig assetPackerConfig = null;
        private Dictionary<string, List<AssetPackerAddressData>> addressRepeatDataDic = new Dictionary<string, List<AssetPackerAddressData>>();
        private void OnEnable()
        {
            assetPackerConfig = AssetPackerUtil.GetAssetPackerConfig();

            foreach(var group in assetPackerConfig.groupDatas)
            {
                foreach(var data in group.assetFiles)
                {
                    if(!addressRepeatDataDic.TryGetValue(data.assetAddress,out List<AssetPackerAddressData> dataList))
                    {
                        dataList = new List<AssetPackerAddressData>();
                        addressRepeatDataDic.Add(data.assetAddress, dataList);
                    }
                    dataList.Add(data);
                }
            }

            if (PlayerSettingsUtility.HasScriptingDefineSymbol(ASSET_BUNDLE_SYMBOL))
            {
                runMode = RunMode.AssetBundle;
            }
        }

        public bool IsAddressRepeated(string address,out List<AssetPackerAddressData> datas)
        {
            if(addressRepeatDataDic.TryGetValue(address,out List<AssetPackerAddressData> list))
            {
                if(list.Count>1)
                {
                    datas = list;
                    return true;
                }
            }

            datas = null;
            return false;
        }

        public bool IsGroupAddressRepeated(AssetPackerGroupData group)
        {
            foreach(var data in group.assetFiles)
            {
                if(IsAddressRepeated(data.assetAddress, out List <AssetPackerAddressData> list))
                {
                    return true;
                }
            }
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
                treeRect.height -= 140;

                operationRect = new Rect(treeRect.x, treeRect.y + treeRect.height + 2, treeRect.width, lastRect.height - treeRect.height - 3);
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
                if (GUILayout.Button(isExpandAll ? "\u25BC" : "\u25BA", EditorStyles.toolbarButton, GUILayout.Width(30)))
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
                        if(searchCategoryIndex!=newIndex)
                        {
                            searchCategoryIndex = newIndex;
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

        private BundleBuildConfig bundleBuildConfig = null;
        private void DrawPackOperation()
        {
            if(bundleBuildConfig == null)
            {
                bundleBuildConfig = AssetPackerUtil.ReadBundleBuildConfig();
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox,GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true));
                {
                    EditorGUILayout.LabelField(Contents.BuildTitleContent, EGUIStyles.MiddleCenterLabel);

                    EditorGUI.BeginChangeCheck();
                    {
                        bundleBuildConfig.bundleOutputDir = EGUILayout.DrawDiskFolderSelection(Contents.BuildOutputDirStr, bundleBuildConfig.bundleOutputDir);
                        bundleBuildConfig.pathFormat = (BundlePathFormatType)EditorGUILayout.EnumPopup(Contents.BuildPathFormatContent, bundleBuildConfig.pathFormat);
                        bundleBuildConfig.cleanupBeforeBuild = EditorGUILayout.Toggle(Contents.BuildCleanup, bundleBuildConfig.cleanupBeforeBuild);
                        bundleBuildConfig.buildTarget = (ValidBuildTarget)EditorGUILayout.EnumPopup(Contents.BuildTargetContent, bundleBuildConfig.buildTarget);
                        bundleBuildConfig.compression = (CompressOption)EditorGUILayout.EnumPopup(Contents.BuildCompressionContent, bundleBuildConfig.compression);
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        AssetPackerUtil.WriteBundleBuildConfig(bundleBuildConfig);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox,GUILayout.Width(160),GUILayout.ExpandHeight(true));
                {
                    EditorGUILayout.LabelField(Contents.OperationTitleContent, EGUIStyles.MiddleCenterLabel);

                    if (GUILayout.Button(Contents.OperationBuildAddressContent))
                    {
                        //AssetAddressUtil.BuildAssetAddressConfig();
                    }
                    EditorGUILayout.Space();
                    if (GUILayout.Button(Contents.OperationCleanBundleNameContent))
                    {
                        AssetPackerUtil.ClearBundleNames(true);
                    }
                    if (GUILayout.Button(Contents.OperationSetBundleNameContent))
                    {
                        AssetPackerUtil.SetAssetBundleNames(assetPackerConfig, bundleBuildConfig.pathFormat,true);
                    }
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Pack Bundle"))
                    {
                        AssetPackerUtil.PackAssetBundle(assetPackerConfig, bundleBuildConfig);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(160));
                {
                    EGUI.BeginGUIBackgroundColor(Color.red);
                    {
                        if (GUILayout.Button(Contents.OperationAutoBuildContent, GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true)))
                        {
                            EditorApplication.delayCall += () =>
                            {
                                //AssetAddressUtil.BuildAssetAddressConfig();

                                AssetPackerUtil.ClearBundleNames(true);
                                AssetPackerUtil.SetAssetBundleNames(assetPackerConfig, bundleBuildConfig.pathFormat, true);
                                AssetPackerUtil.PackAssetBundle(assetPackerConfig, bundleBuildConfig);
                            };
                        }
                    }
                    EGUI.EndGUIBackgroundColor();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
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

        private bool FilterAddressData(AssetPackerAddressData addressData)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            bool isValid = false;
            if (searchCategoryIndex == 0 || searchCategoryIndex == 1)
            {
                if (!string.IsNullOrEmpty(addressData.assetAddress))
                {
                    isValid = addressData.assetAddress.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            if (!isValid)
            {
                if (searchCategoryIndex == 0 || searchCategoryIndex == 2)
                {
                    if (!string.IsNullOrEmpty(addressData.assetPath))
                    {
                        isValid = addressData.assetPath.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            if (!isValid)
            {
                if (searchCategoryIndex == 0 || searchCategoryIndex == 3)
                {
                    if (!string.IsNullOrEmpty(addressData.bundlePath))
                    {
                        string bPath = $"{addressData.bundlePath} {addressData.bundlePathMd5}";
                        isValid = bPath.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            if (!isValid)
            {
                string label = string.Join(",", addressData.labels);
                if (searchCategoryIndex == 0 || searchCategoryIndex == 4)
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        isValid = label.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            }
            return isValid;
        }

        private void SetTreeModel()
        {
            TreeModel<TreeElementWithData<AssetPackerTreeData>> treeModel = assetPackerTreeView.treeModel;
            TreeElementWithData<AssetPackerTreeData> treeModelRoot = treeModel.root;
            treeModelRoot.children?.Clear();

            for(int i =0;i<assetPackerConfig.groupDatas.Count;++i)
            {
                AssetPackerGroupData groupData = assetPackerConfig.groupDatas[i];
                AssetPackerTreeData groupTreeData = new AssetPackerTreeData();
                groupTreeData.groupData = groupData;

                TreeElementWithData<AssetPackerTreeData> groupElementData = new TreeElementWithData<AssetPackerTreeData>(
                    groupTreeData, "", 0, (i + 1) * 100000);

                treeModel.AddElement(groupElementData, treeModelRoot, treeModelRoot.hasChildren ? treeModelRoot.children.Count : 0);

                for(int j=0;j<groupData.assetFiles.Count;++j)
                {
                    AssetPackerAddressData addressData = groupData.assetFiles[j];
                    if(FilterAddressData(addressData))
                    {
                        AssetPackerTreeData elementTreeData = new AssetPackerTreeData();
                        elementTreeData.groupData = groupData;
                        elementTreeData.dataIndex = j;

                        TreeElementWithData<AssetPackerTreeData> elementData = new TreeElementWithData<AssetPackerTreeData>(
                                elementTreeData, "", 1, (i + 1) * 100000 + (j + 1));

                        treeModel.AddElement(elementData, groupElementData, groupElementData.hasChildren ? groupElementData.children.Count : 0);
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
            internal static GUIContent BuildTargetContent = new GUIContent("Build Target");
            internal static GUIContent BuildCompressionContent = new GUIContent("Compression");

            internal static GUIContent OperationTitleContent = new GUIContent("Operation");

            internal static GUIContent OperationBuildAddressContent = new GUIContent("Build Address");
            internal static GUIContent OperationCleanBundleNameContent = new GUIContent("Clean Bundle Name");
            internal static GUIContent OperationSetBundleNameContent = new GUIContent("Set Bundle Name");
            internal static GUIContent OperationBuildContent = new GUIContent("Pack Bundle");

            internal static GUIContent OperationAutoBuildContent = new GUIContent("Auto Pack Bundle");
        }
    }
}
