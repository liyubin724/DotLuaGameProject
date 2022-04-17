using DotEngine.Generic;
using System.Collections.Generic;
using SystemObject = System.Object;

namespace DotEditor.GUIExtension.DataGrid
{
    public class GridViewModel
    {
        public GridViewData RootData { get; private set; }

        private IntIDCreator m_IDCreator = new IntIDCreator(0);
        private Dictionary<int, GridViewData> m_IDToDataDic = new Dictionary<int, GridViewData>();

        public GridViewModel()
        {
            RootData = new GridViewData(string.Empty)
            {
                ID = -1,
                Depth = -1,
                IsExpand = true,
            };
        }

        public virtual void Clear()
        {
            m_IDToDataDic.Clear();
            RootData.Children.Clear();
        }

        public GridViewData GetDataByID(int id)
        {
            if (m_IDToDataDic.TryGetValue(id, out var data))
            {
                return data;
            }
            return null;
        }

        public GridViewData GetDataByUserdata(SystemObject sysObj)
        {
            if(sysObj!=null)
            {
                foreach(var kvp in m_IDToDataDic)
                {
                    if(kvp.Value.Userdata == sysObj)
                    {
                        return kvp.Value;
                    }
                }
            }

            return null;
        }

        public void RemoveData(GridViewData data)
        {
            GridViewData parentData = data.Parent;
            parentData.Children.Remove(data);
            data.Parent = null;

            List<GridViewData> dataList = new List<GridViewData>();
            GetDeepChilds(data,ref dataList,true);

            foreach (var child in dataList)
            {
                m_IDToDataDic.Remove(child.ID);
            }
        }

        public void RemoveDataByID(int id)
        {
            if (m_IDToDataDic.TryGetValue(id, out var data))
            {
                RemoveData(data);
            }
        }

        public void AddData(GridViewData data)
        {
            AddChildData(RootData, data);
        }

        public void InsertData(int index,GridViewData data)
        {
            InsertChildData(index, RootData, data);
        }

        public void AddChildData(GridViewData parentData, GridViewData childData)
        {
            parentData.IsExpand = true;
            parentData.Children.Add(childData);

            childData.Parent = parentData;
            childData.ID = m_IDCreator.GetNextID();
            childData.Depth = parentData.Depth + 1;

            m_IDToDataDic.Add(childData.ID, childData);
        }

        public void InsertChildData(int index,GridViewData parentData,GridViewData childData)
        {
            parentData.IsExpand = true;
            parentData.Children.Insert(index,childData);

            childData.Parent = parentData;
            childData.ID = m_IDCreator.GetNextID();
            childData.Depth = parentData.Depth + 1;

            m_IDToDataDic.Add(childData.ID, childData);
        }

        public virtual bool HasChild(GridViewData data)
        {
            return data.Children.Count > 0;
        }

        public virtual GridViewData[] GetChilds(GridViewData data)
        {
            if (data.IsExpand)
            {
                return data.Children.ToArray();
            }
            return new GridViewData[0];
        }

        internal void ExpandDatas(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    if (m_IDToDataDic.TryGetValue(id, out var data))
                    {
                        OnDataExpand(data);
                        data.IsExpand = true;
                    }
                }
            }
        }

        internal void CollapseDatas(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                List<GridViewData> allChilds = new List<GridViewData>();
                foreach (var id in ids)
                {
                    if (m_IDToDataDic.TryGetValue(id, out var data))
                    {
                        data.IsExpand = false;
                        
                        GetDeepChilds(data, ref allChilds, false);

                        data.Children.Clear();
                    }
                }

                foreach (var child in allChilds)
                {
                     m_IDToDataDic.Remove(child.ID);
                }
            }
        }

        protected virtual void OnDataExpand(GridViewData data) { }
        protected virtual void OnDataCollapse(GridViewData data) { }

        private void GetDeepChilds(GridViewData data, ref List<GridViewData> dataList, bool isIncludeSelf = false)
        {
            if (isIncludeSelf)
            {
                dataList.Add(data);
            }
            foreach (var child in data.Children)
            {
                GetDeepChilds(child, ref dataList, true);
            }
        }
    }
}
