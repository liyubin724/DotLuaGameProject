using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.DataGrid
{
    public class EGUIGridView
    {
        private GridTreeView m_TreeView = null;
        
        public TreeViewState ViewState { get; private set; }
        public GridViewModel ViewModel { get; private set; }
        public GridViewHeader ViewHeader { get; private set; }

        public EGUIGridView(GridViewModel model,string[] columnTitles)
        {
            ViewState = new TreeViewState();
            ViewModel = model;
            ViewHeader = new GridViewHeader(columnTitles);

            m_TreeView = new GridTreeView(ViewState, ViewHeader.GetTreeViewHeader(), ViewModel)
            {
                OnDrawColumnItem = OnDrawColumnItem,
                OnGetRowHeight = GetRowHeight,
                OnItemContextClicked = OnItemContextClicked,
                OnItemDoubleClicked = OnItemDoubleClicked,
            };
            m_TreeView.Reload();

            m_TreeView.multiColumnHeader.ResizeToFit();
        }

        public EGUIGridView(GridViewModel model,GridViewHeader header)
        {
            ViewState = new TreeViewState();
            ViewModel = model;
            ViewHeader = header;

            m_TreeView = new GridTreeView(ViewState, header.GetTreeViewHeader(), ViewModel)
            {
                OnDrawColumnItem = OnDrawColumnItem,
                OnGetRowHeight = GetRowHeight,
                OnItemContextClicked = OnItemContextClicked,
                OnItemDoubleClicked  = OnItemDoubleClicked,
            };
            m_TreeView.Reload();
            m_TreeView.multiColumnHeader.ResizeToFit();
        }

        protected virtual void OnDrawColumnItem(Rect rect,int columnIndex,GridViewData columnItemData)
        {
            EditorGUI.LabelField(rect, $"{columnItemData.DisplayName}-{columnIndex}");
        }

        protected virtual float GetRowHeight(GridViewData itemData)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        protected virtual void OnItemContextClicked(GridViewData itemData)
        {

        }

        protected virtual void OnItemDoubleClicked(GridViewData itemData)
        {

        }

        public void Reload()
        {
            m_TreeView?.Reload();
        }

        public void OnGUILayout()
        {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            m_TreeView?.OnGUI(rect);
        }

        public void OnGUI(Rect rect)
        {
            m_TreeView?.OnGUI(rect);
        }
    }
}
