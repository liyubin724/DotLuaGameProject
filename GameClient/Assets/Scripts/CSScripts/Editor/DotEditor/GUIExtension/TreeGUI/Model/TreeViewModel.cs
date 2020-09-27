using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class TreeViewModel
    {
        public TreeViewData RootData { get; private set; }
        private int m_IncreaseDataID = 0;
        private Dictionary<int, TreeViewData> m_IDToDataDic = new Dictionary<int, TreeViewData>();

        public TreeViewModel()
        {
            RootData = new TreeViewData()
            {
                Depth = -1,
                ID = -1,
                IsExpand = true,
            };
        }

        public void Clear()
        {
            m_IDToDataDic.Clear();
            RootData.Children.Clear();
        }

        public TreeViewData Get(int id)
        {
            if(m_IDToDataDic.TryGetValue(id, out var data))
            {
                return data;
            }
            return null;
        }

        public void AddChild(TreeViewData parent, TreeViewData data)
        {
            parent.Children.Add(data);
            data.Parent = parent;

            data.ID = ++m_IncreaseDataID;
            data.Depth = parent.Depth + 1;

            m_IDToDataDic.Add(data.ID, data);
        }

        public void RemoveChild(TreeViewData data)
        {
            if(data == RootData || data.Parent == null)
            {
                return;
            }
            foreach(var child in data.Children)
            {
                RemoveChild(child);
            }
            data.Parent.Children.Remove(data);
            m_IDToDataDic.Remove(data.ID);
            data.Parent = null;
        }

        public virtual bool HasChild(TreeViewData data)
        {
            return data.Children.Count > 0;
        }

        public virtual TreeViewData[] GetChilds(TreeViewData data)
        {
            if (data.IsExpand)
            {
                return data.Children.ToArray();
            }
            return new TreeViewData[0];
        }

        public void ExpandDatas(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    if (m_IDToDataDic.TryGetValue(id, out var data))
                    {
                        OnExpandData(data);
                        data.IsExpand = true;
                    }
                }
            }
        }

        protected virtual void OnExpandData(TreeViewData data)
        { }

        public void CollapseDatas(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    if (m_IDToDataDic.TryGetValue(id, out var data))
                    {
                        data.IsExpand = false;
                        var childs = data.Children.ToArray();
                        foreach(var child in childs)
                        {
                            RemoveChild(child);
                        }
                        OnCollapseData(data);
                    }
                }
            }
        }

        protected virtual void OnCollapseData(TreeViewData data)
        { }
    }
}
