using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.DataGrid
{
    public class EGUITreeView
    {
        private static float HEADER_HEIGHT = 30;

        protected GridTreeView treeView = null;

        public string HeaderContent { get; set; } = null;
        public TreeViewState ViewState { get; private set; }
        public GridViewModel ViewModel { get; private set; }

        protected EGUITreeView() 
        {
            ViewState = new TreeViewState();
            ViewModel = new GridViewModel();

            treeView = new GridTreeView(ViewState, ViewModel)
            {
                OnDrawRowItem = OnDrawRowItem,
                OnGetRowHeight = GetRowHeight,
                OnItemContextClicked = OnItemContextClicked,
                OnItemDoubleClicked = OnItemDoubleClicked,
                OnItemSelectedChanged = OnItemSelectedChanged,

                IsMultiSelect = false,
            };
            treeView.Reload();
        }

        public EGUITreeView(GridViewModel model)
        {
            ViewState = new TreeViewState();
            ViewModel = model;

            treeView = new GridTreeView(ViewState, ViewModel)
            {
                OnDrawRowItem = OnDrawRowItem,
                OnGetRowHeight = GetRowHeight,
                OnItemContextClicked = OnItemContextClicked,
                OnItemDoubleClicked = OnItemDoubleClicked,
                OnItemSelectedChanged = OnItemSelectedChanged,
            };
            treeView.Reload();
        }

        public T GetViewModel<T>()where T:GridViewModel
        {
            return (T)ViewModel;
        }

        public void Reload(int[] expandIDs, int[] selectedIDs)
        {
            SetExpand(expandIDs);
            SetSelection(selectedIDs);
        }

        public void SetSelection(int[] selectedIDs)
        {
            treeView?.SetSelection(selectedIDs ?? new int[0], TreeViewSelectionOptions.FireSelectionChanged);
        }

        public void SetExpand(int[] expandIDs)
        {
            treeView?.SetExpanded(expandIDs ?? new int[0]);
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

        protected virtual void OnItemSelectedChanged(GridViewData[] itemDatas)
        {

        }

        public void Reload()
        {
            treeView?.Reload();
        }

        public virtual void OnGUILayout()
        {
            if (!string.IsNullOrEmpty(HeaderContent))
            {
                EGUILayout.DrawBoxHeader(HeaderContent, EGUIStyles.BoxedHeaderCenterStyle,GUILayout.ExpandWidth(true));
            }
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            treeView?.OnGUI(rect);
        }

        public virtual void OnGUI(Rect rect)
        {
            if (!string.IsNullOrEmpty(HeaderContent))
            {
                EGUI.DrawBoxHeader(new Rect(rect.x, rect.y, rect.width, HEADER_HEIGHT), HeaderContent, EGUIStyles.BoxedHeaderCenterStyle);
            }

            rect.height -= HEADER_HEIGHT;

            treeView?.OnGUI(rect);
        }
    }
}
