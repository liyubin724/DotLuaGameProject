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
        private string[] m_IgnoreExtensions = new string[0];
        private AssetDependencyConfig dependencyConfig = null;

        public void SetDependencyConfig(AssetDependencyConfig config)
        {
            dependencyConfig = config;
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

        public int[] ShowAssets(string[] assetPaths)
        {
            Clear();
            List<int> ids = new List<int>();
            if(assetPaths!=null && assetPaths.Length>0)
            {
                foreach (var assetPath in assetPaths)
                {
                    if (IsAssetIgnored(assetPath))
                    {
                        continue;
                    }

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

            int count = (from d in adData.directlyDepends
                         where !IsAssetIgnored(d)
                         select d).Count();

            return count > 0;
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

                    if (IsAssetIgnored(assetPath))
                    {
                        continue;
                    }

                    AssetDependency childADData = dependencyConfig.GetData(assetPath);
                    var childData = new GridViewData(assetPath, childADData);
                    AddChildData(data, childData);
                }
            }
        }

        private bool IsAssetIgnored(string assetPath)
        {
            return Array.IndexOf(m_IgnoreExtensions, Path.GetExtension(assetPath).ToLower()) >= 0;
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
