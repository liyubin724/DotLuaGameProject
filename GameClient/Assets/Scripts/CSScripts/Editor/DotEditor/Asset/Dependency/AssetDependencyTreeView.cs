using DotEditor.GUIExtension;
using DotEditor.GUIExtension.TreeGUI;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    internal class AssetDependencyTreeViewModel : TreeViewModel
    {
        private string[] m_IgnoreExtensions = new string[0];

        public override bool HasChild(TreeViewData data)
        {
            AssetDependencyData adData = data.GetData<AssetDependencyData>();

            int count = (from d in adData.directlyDepends
                         where Array.IndexOf(m_IgnoreExtensions, Path.GetExtension(d).ToLower()) < 0
                         select d).Count();

            return count > 0;
        }

        public void SetIgnoreExtension(string[] extensions)
        {
            if(extensions!=null && extensions.Length>0)
            {
                m_IgnoreExtensions = (from ie in extensions select ie.ToLower()).ToArray();
            }else
            {
                m_IgnoreExtensions = new string[0];
            }
        }

        private bool IsAssetIgnored(string assetPath)
        {
            return Array.IndexOf(m_IgnoreExtensions, Path.GetExtension(assetPath).ToLower()) >= 0;
        }

        public int[] ShowDependency(string[] assetPaths)
        {
            Clear();

            List<int> ids = new List<int>();
            foreach(var assetPath in assetPaths)
            {
                if(IsAssetIgnored(assetPath))
                {
                    continue;
                }

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

        public void ShowSelected(string selectedAssetPath,out List<int> expandIDs,out List<int> selectedIDs)
        {
            selectedIDs = new List<int>();
            expandIDs = new List<int>();
            foreach(var child in RootData.Children)
            {
                CreateSelectedAsset(child, selectedAssetPath, selectedIDs);
            }

            selectedIDs = selectedIDs.Distinct().ToList();

            foreach(var id in selectedIDs)
            {
                TreeViewData parentData = Get(id).Parent;
                while(parentData!= RootData)
                {
                    expandIDs.Add(parentData.ID);
                    parentData = parentData.Parent;
                }
            }
            expandIDs = expandIDs.Distinct().ToList();
        }

        private void CreateSelectedAsset(TreeViewData data,string selectedAssetPath,List<int> selectedIDs)
        {
            AssetDependencyData adData = data.GetData<AssetDependencyData>();
            if(adData.assetPath == selectedAssetPath)
            {
                selectedIDs.Add(data.ID);
                return;
            }
            if (adData.allDepends != null && Array.IndexOf(adData.allDepends, selectedAssetPath) >= 0)
            {
                if(!data.IsExpand)
                {
                    foreach(var childAssetPath in adData.directlyDepends)
                    {
                        if(IsAssetIgnored(childAssetPath))
                        {
                            continue;
                        }
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
                    CreateSelectedAsset(childData, selectedAssetPath, selectedIDs);
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

                    if (IsAssetIgnored(assetPath))
                    {
                        continue;
                    }

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

        protected override void DoubleClickedItem(int id)
        {
            base.DoubleClickedItem(id);
            AssetDependencyData adData = Model.Get(id).GetData< AssetDependencyData>();
            SelectionUtility.PingObject(adData.assetPath);
            SelectionUtility.ActiveObject(adData.assetPath);
        }

        protected override void DrawTreeViewItem(Rect rect, EGUITreeViewItem item)
        {
            AssetDependencyData adData = item.ItemData.GetData<AssetDependencyData>();

            Rect iconRect = new Rect(rect.x, rect.y, rect.height, rect.height);
            UnityObject assetObj = AssetDatabase.LoadAssetAtPath(adData.assetPath, typeof(UnityObject));
            Texture2D previewIcon = null;
            if(assetObj == null)
            {
                previewIcon = EGUIResources.ErrorIcon;
            }else if (!AssetPreview.IsLoadingAssetPreview(assetObj.GetInstanceID()))
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
