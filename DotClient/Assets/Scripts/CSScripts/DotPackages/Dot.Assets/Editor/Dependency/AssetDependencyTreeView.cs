using DotEditor.GUIExtension;
using DotEditor.GUIExtension.DataGrid;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    internal class AssetDependencyTreeViewModel : GridViewModel
    {
        private AssetDependencyConfig dependencyConfig = null;

        public void SetDependencyConfig(AssetDependencyConfig config)
        {
            dependencyConfig = config;
        }

        public int[] ShowAssets(string[] assetPaths)
        {
            Clear();
            List<int> ids = new List<int>();
            if(assetPaths!=null && assetPaths.Length>0)
            {
                foreach (var assetPath in assetPaths)
                {
                    AssetDependency adData = dependencyConfig.GetData(assetPath);
                    GridViewData viewData = new GridViewData(assetPath, adData);
                    AddChildData(RootData, viewData);

                    ids.Add(viewData.ID);
                }
            }
            return ids.ToArray();
        }

        public override bool HasChild(GridViewData data)
        {
            AssetDependency adData = data.GetData<AssetDependency>();
            if(adData == null)
            {
                return false;
            }

            return adData.directlyDepends.Length > 0;
        }

        protected override void OnDataCollapse(GridViewData data)
        {
            if(!data.IsExpand)
            {
                return;
            }
            data.Children.Clear();
        }

        protected override void OnDataExpand(GridViewData data)
        {
            if(data.IsExpand)
            {
                return;
            }
            AssetDependency adData = data.GetData<AssetDependency>();
            if(adData.directlyDepends!=null && adData.directlyDepends.Length>0)
            {
                for(int i =0;i<adData.directlyDepends.Length;++i)
                {
                    string assetPath = adData.directlyDepends[i];

                    AssetDependency childADData = dependencyConfig.GetData(assetPath);
                    if(childADData == null)
                    {
                        Debug.Log("SSSSSSSSSSS->" + assetPath);
                    }
                    var childData = new GridViewData(assetPath, childADData);
                    AddChildData(data, childData);
                }
            }
        }
    }

    internal class AssetDependencyTreeView : EGUITreeView
    {
        public AssetDependencyTreeView(AssetDependencyTreeViewModel model) : base(model)
        {
            Reload();
        }

        protected override float GetRowHeight(GridViewData itemData)
        {
            return 32.0f;
        }

        protected override void OnItemDoubleClicked(GridViewData itemData)
        {
            AssetDependency adData = itemData.GetData<AssetDependency>();
            if (adData == null)
            {
                return;
            }
            SelectionUtility.PingObject(adData.assetPath);
        }

        protected override void OnDrawRowItem(Rect rect, GridViewData itemData)
        {
            AssetDependency adData = itemData.GetData<AssetDependency>();
            if(adData == null)
            {
                EditorGUI.LabelField(rect, "The data is null");
                return;
            }

            Rect iconRect = new Rect(rect.x, rect.y, rect.height, rect.height);
            UnityObject assetObj = adData.cachedUObject;
            if(assetObj == null)
            {
                assetObj = AssetDatabase.LoadAssetAtPath(adData.assetPath, typeof(UnityObject));
                adData.cachedUObject = assetObj;
            }
            Texture2D previewIcon = adData.cachedPreview;
            if(previewIcon == null)
            {
                previewIcon = EGUIResources.GetAssetPreview(assetObj);
                adData.cachedPreview = previewIcon;
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
