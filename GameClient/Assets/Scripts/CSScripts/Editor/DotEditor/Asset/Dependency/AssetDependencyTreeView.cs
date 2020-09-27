using DotEditor.GUIExtension;
using DotEditor.GUIExtension.TreeGUI;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    internal class AssetDependencyTreeViewModel : TreeViewModel
    {
        public override bool HasChild(TreeViewData data)
        {
            AssetDependencyData adData = data.GetData<AssetDependencyData>();
            return adData.directlyDepends!=null && adData.directlyDepends.Length > 0;
        }

        public int[] ShowAssetDependency(string[] assetPaths)
        {
            Clear();

            List<int> ids = new List<int>();
            foreach(var assetPath in assetPaths)
            {
                AssetDependencyData adData = AssetDependencyUtil.GetDependencyData(assetPath);
                TreeViewData viewData = new TreeViewData()
                {
                    UserData = adData,
                    DisplayName = assetPath,
                };
                AddChild(RootData, viewData);

                ids.Add(viewData.ID);
            }
            return ids.ToArray();
        }

        public int[] ShowSelectedAssets(string selectedAssetPath)
        {
            List<int> ids = new List<int>();

            foreach(var child in RootData.Children)
            {
                CreateSelectedAsset(child, selectedAssetPath, ids);
            }

            return ids.Distinct().ToArray();
        }

        private void CreateSelectedAsset(TreeViewData data,string selectedAssetPath,List<int> ids)
        {
            AssetDependencyData adData = data.GetData<AssetDependencyData>();
            if(adData.assetPath == selectedAssetPath)
            {
                ids.Add(data.ID);
                return;
            }
            if (adData.allDepends != null && Array.IndexOf(adData.allDepends, selectedAssetPath) >= 0)
            {
                if(!data.IsExpand)
                {
                    foreach(var childAssetPath in adData.directlyDepends)
                    {
                        AssetDependencyData childADData = AssetDependencyUtil.GetDependencyData(childAssetPath);
                        TreeViewData viewData = new TreeViewData()
                        {
                            UserData = childADData,
                            DisplayName = childAssetPath,
                        };
                        AddChild(data, viewData);
                    }
                    data.IsExpand = true;
                }

                foreach(var childData in data.Children)
                {
                    CreateSelectedAsset(childData, selectedAssetPath, ids);
                }
            }
        }

        protected override void OnExpandData(TreeViewData data)
        {
            if(data.IsExpand)
            {
                return;
            }
            AssetDependencyData adData = data.GetData<AssetDependencyData>();
            if(adData.directlyDepends!=null && adData.directlyDepends.Length>0)
            {
                for(int i =0;i<adData.directlyDepends.Length;++i)
                {
                    string assetPath = adData.directlyDepends[i];
                    AssetDependencyData childADData = AssetDependencyUtil.GetDependencyData(assetPath);
                    var childData = new TreeViewData()
                    {
                        DisplayName = assetPath,
                        UserData = childADData,
                    };
                    AddChild(data, childData);
                }
            }    
        }
    }

    internal class AssetDependencyTreeView : EGUITreeView
    {
        public AssetDependencyTreeView(TreeViewState state,AssetDependencyTreeViewModel model) : base(state,model)
        {
            rowHeight = 32;
            Reload();
        }

        public void Reload(int[] expandIDs,int[] selectedIDs)
        {
            SetExpanded(expandIDs??new int[0]);
            SetSelection(selectedIDs??new int[0], TreeViewSelectionOptions.FireSelectionChanged);
        }

        protected override void DrawTreeViewItem(Rect rect, EGUITreeViewItem item)
        {
            AssetDependencyData adData = item.ItemData.GetData<AssetDependencyData>();

            Rect iconRect = new Rect(rect.x, rect.y, rect.height, rect.height);
            UnityObject assetObj = AssetDatabase.LoadAssetAtPath(adData.assetPath, typeof(UnityObject));
            Texture2D previewIcon = null;
            if (!AssetPreview.IsLoadingAssetPreview(assetObj.GetInstanceID()))
            {
                previewIcon = AssetPreview.GetAssetPreview(assetObj);
            }
            if (previewIcon == null)
            {
                previewIcon = EGUIResources.MakeColorTexture((int)iconRect.width, (int)iconRect.height, Color.grey);
            }
            GUI.DrawTexture(iconRect, previewIcon, ScaleMode.ScaleAndCrop);

            if (Event.current.type == EventType.MouseUp && iconRect.Contains(Event.current.mousePosition))
            {
                SelectionUtility.PingObject(assetObj);
                SetSelection(new int[] { item.id }, TreeViewSelectionOptions.RevealAndFrame);
            }

            Rect labelRect = new Rect(iconRect.x + iconRect.width, iconRect.y, rect.width - iconRect.width, iconRect.height);
            EditorGUI.LabelField(labelRect, adData.assetPath, EGUIStyles.MiddleLeftLabelStyle);

            if (assetObj is Texture)
            {
                Rect memorySizeRect = new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height);
                long memorySize = AssetDatabaseUtility.GetTextureStorageSize(assetObj as Texture);
                EditorGUI.LabelField(memorySizeRect, EditorUtility.FormatBytes(memorySize));
            }
        }
    }
}
