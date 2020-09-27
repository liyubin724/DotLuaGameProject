using DotEditor.GUIExtension;
using DotEditor.GUIExtension.TreeGUI;
using DotEditor.Utilities;
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

        protected override void OnExpandData(TreeViewData data)
        {
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

        //internal void SetIgnoreAssets(string[] ignoreExt)
        //{
        //    m_IgnoreAssetExtensions = ignoreExt;
        //    if(treeModel.root.children != null && treeModel.root.children.Count>0)
        //    {
        //        ShowDependency(m_MainAssetPaths, m_SelectedAssetPaths);
        //    }
        //}

        //private bool IsIgnoreAsset(string assetPath)
        //{
        //    if(m_IgnoreAssetExtensions == null || m_IgnoreAssetExtensions.Length == 0)
        //    {
        //        return false;
        //    }
        //    var extension = Path.GetExtension(assetPath).ToLower();

        //    return Array.IndexOf(m_IgnoreAssetExtensions, extension) >= 0;
        //}

        //public void RefreshDependency()
        //{
        //    ShowDependency(m_MainAssetPaths, m_SelectedAssetPaths);
        //}

        //internal void ShowDependency(string[] assetPaths, string[] selectedAssetPaths = null)
        //{
        //    m_MainAssetPaths = assetPaths;
        //    m_SelectedAssetPaths = selectedAssetPaths;

        //    m_ElementToAssetPathDic.Clear();
        //    treeModel.root.children?.Clear();

        //    if(assetPaths!=null && assetPaths.Length>0)
        //    {
        //        foreach(var assetPath in assetPaths)
        //        {
        //            CreateDependencyChildNode(treeModel.root, assetPath);
        //        }
        //    }
        //    //ExpandAll();
            
        //    if(selectedAssetPaths!=null && selectedAssetPaths.Length>0)
        //    {
        //        int[] selectedItemIds = (from d in m_ElementToAssetPathDic where Array.IndexOf(selectedAssetPaths, d.Value) >= 0 select d.Key).ToArray();
        //        SetSelection(selectedItemIds, TreeViewSelectionOptions.RevealAndFrame);
        //    }
        //    SetFocus();
        //    Reload();
        //}

        //private void CreateDependencyChildNode(TreeElementWithData<TreeViewData> parentNode, string assetPath)
        //{
        //    if(IsIgnoreAsset(assetPath))
        //    {
        //        return;
        //    }

        //    AssetDependencyData data = AssetDependencyUtil.GetDependencyData(assetPath);
        //    TreeElementWithData<TreeViewData> elementData = new TreeElementWithData<TreeViewData>(new TreeViewData()
        //    {
        //        dependencyData = data,
        //    }, data.assetPath, parentNode.depth + 1, m_TreeViewElementIndex);

        //    m_ElementToAssetPathDic.Add(elementData.id, elementData.name);

        //    m_TreeViewElementIndex++;
        //    treeModel.AddElement(elementData, parentNode, parentNode.hasChildren ? parentNode.children.Count : 0);

        //    if(IsRepeatInTreeView((TreeElementWithData<TreeViewData>)parentNode.parent,assetPath))
        //    {
        //        elementData.Data.isRepeatInTreeView = true;
        //        return;
        //    }

        //    if (data.directlyDepends != null && data.directlyDepends.Length > 0)
        //    {
        //        foreach (var path in data.directlyDepends)
        //        {
        //            CreateDependencyChildNode(elementData, path);
        //        }
        //    }
        //}

        //private bool IsRepeatInTreeView(TreeElementWithData<TreeViewData> parentNode, string assetPath)
        //{
        //    if(parentNode == null || parentNode == treeModel.root)
        //    {
        //        return false;
        //    }
        //     if(parentNode.Data.dependencyData.assetPath == assetPath)
        //    {
        //        return true;
        //    }else
        //    {
        //        return IsRepeatInTreeView((TreeElementWithData<TreeViewData>)parentNode.parent, assetPath);
        //    }

        //}
    }
}
