using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.DataGrid
{
    public class EGUIGridView
    {
        private static float HEADER_HEIGHT = 30;

        private GridTreeView m_TreeView = null;

        public string HeaderContent { get; set; } = null;

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
                OnItemSelectedChanged = OnItemSelectedChanged,

                IsMultiSelect = false,
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
        protected virtual void OnItemSelectedChanged(GridViewData[] itemDatas)
        {

        }

        public void Reload()
        {
            m_TreeView?.Reload();
        }

        public void OnGUILayout()
        {
            if(!string.IsNullOrEmpty(HeaderContent))
            {
                EGUILayout.DrawBoxHeader(HeaderContent, EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
            }
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            m_TreeView?.OnGUI(rect);
        }

        public void OnGUI(Rect rect)
        {
            if (!string.IsNullOrEmpty(HeaderContent))
            {
                EGUI.DrawBoxHeader(new Rect(rect.x, rect.y, rect.width, HEADER_HEIGHT), HeaderContent, EGUIStyles.BoxedHeaderCenterStyle);
            }

            rect.height -= HEADER_HEIGHT;

            m_TreeView?.OnGUI(rect);
        }
    }
}
