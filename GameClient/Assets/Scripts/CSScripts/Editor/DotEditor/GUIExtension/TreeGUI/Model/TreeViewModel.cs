using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class TreeViewModel
    {
        public TreeViewData RootData { get; private set; }

        private int m_IncreaseDataID = 0;
        private int GenerateUniqueID()
        {
            return ++m_IncreaseDataID;
        }

        private Dictionary<int, TreeViewData> m_IDToDataDic = new Dictionary<int, TreeViewData>();
        public TreeViewModel(TreeViewData root)
        {
            RootData = root;
            root.IsExpand = true;

            m_IDToDataDic.Add(root.ID, root);
        }

        public void AddChild(TreeViewData parent,TreeViewData data)
        {
            parent.Children.Add(data);
            data.Parent = parent;

            data.ID = GenerateUniqueID();
            data.Depth = parent.Depth + 1;

            m_IDToDataDic.Add(data.ID, data);
        }

        public virtual bool HasChild(TreeViewData data)
        {
            return data.Children.Count > 0;
        }

        public virtual TreeViewData[] GetChilds(TreeViewData data)
        {
            return data.Children.ToArray();
        }

        public void CollapseDatas(int[] ids)
        {
            if(ids!=null && ids.Length>0)
            {
                foreach(var id in ids)
                {
                    if(m_IDToDataDic.TryGetValue(id, out var data))
                    {
                        data.IsExpand = false;
                        data.Children.Clear();
                        OnCollapseData(data);
                    }
                }
            }
        }

        protected virtual void OnCollapseData(TreeViewData data)
        { }

        public void ExpandDatas(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    if (m_IDToDataDic.TryGetValue(id, out var data))
                    {
                        data.IsExpand = true;
                        OnExpandData(data);
                    }
                }
            }
        }

        protected virtual void OnExpandData(TreeViewData data)
        { }


    }
}
