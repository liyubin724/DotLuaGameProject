using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.MultiColumnHeaderState;

namespace DotEditor.GUIExtension.DataGrid
{
    public class GridViewColumn
    {
        public string Title { get; private set; } = string.Empty;
        public string Tooltip { get; private set; } = string.Empty;
        public bool AutoResize { get; set; } = true;
        public int MaxWidth { get; set; } = int.MaxValue;
        public int MinWidth { get; set; } = 40;

        public GridViewColumn(string title)
        {
            Title = title;
        }

        public GridViewColumn(string title, string tooltip)
        {
            Title = title;
            Tooltip = tooltip;
        }

        public static implicit operator Column(GridViewColumn column)
        {
            Column c = new Column()
            {
                canSort = false,
                headerContent = new GUIContent(column.Title, column.Tooltip),
                autoResize = column.AutoResize,
                headerTextAlignment = TextAlignment.Center,
                maxWidth = column.MaxWidth,
                minWidth = column.MinWidth,
            };

            return c;
        }
    }

    public class GridViewHeader
    {
        public List<GridViewColumn> m_Columns = new List<GridViewColumn>();

        public GridViewHeader()
        {
        }

        public GridViewHeader(string[] titles)
        {
            foreach(var title in titles)
            {
                m_Columns.Add(new GridViewColumn(title));
            }
        }

        public void AddColumn(string title,string tooltip = "")
        {
            m_Columns.Add(new GridViewColumn(title, tooltip));
        }

        public string GetColumnTitle(int index)
        {
            if (index < 0 || index >= m_Columns.Count)
            {
                return string.Empty;
            }
            return m_Columns[index].Title;
        }

        private Column[] GetTreeViewColumns()
        {
            Column[] columns = new Column[m_Columns.Count];
            for(int i =0;i<m_Columns.Count;++i)
            {
                columns[i] = m_Columns[i];
            }
            return columns;
        }

        public MultiColumnHeader GetTreeViewHeader()
        {
            return new MultiColumnHeader(new MultiColumnHeaderState(GetTreeViewColumns()));
        }
    }
}
