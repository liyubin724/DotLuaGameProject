using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.DataGrid
{
    public class EGUITreeView
    {
        private GridTreeView m_TreeView = null;

        public TreeViewState ViewState { get; private set; }
        public GridViewModel ViewModel { get; private set; }

        public EGUITreeView(GridViewModel model)
        {
            ViewState = new TreeViewState();
            ViewModel = model;

            m_TreeView = new GridTreeView(ViewState, ViewModel)
            {
                OnDrawRowItem = OnDrawRowItem,
                OnGetRowHeight = GetRowHeight,
                OnItemContextClicked = OnItemContextClicked,
                OnItemDoubleClicked = OnItemDoubleClicked,
            };
            m_TreeView.Reload();
        }

        public T GetViewModel<T>()where T:GridViewModel
        {
            return (T)ViewModel;
        }

        public void Reload(int[] expandIDs, int[] selectedIDs)
        {
            m_TreeView?.SetExpanded(expandIDs ?? new int[0]);
            m_TreeView?.SetSelection(selectedIDs ?? new int[0], TreeViewSelectionOptions.FireSelectionChanged);
        }

        protected virtual void OnDrawRowItem(Rect rect,GridViewData itemData)
        {
            EditorGUI.LabelField(rect, itemData.DisplayName);
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
