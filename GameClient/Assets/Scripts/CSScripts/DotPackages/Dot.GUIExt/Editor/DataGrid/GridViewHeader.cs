using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.MultiColumnHeaderState;

namespace DotEditor.GUIExt.DataGrid
{
    public class GridViewColumn
    {
        public string Title { get; private set; } = string.Empty;
        public string Tooltip { get; private set; } = string.Empty;
        public bool AutoResize { get; set; } = true;
        public int MaxWidth { get; set; } = int.MaxValue;
        public int MinWidth { get; set; } = 40;
        public int Width { get; set; }

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
            };
            if(column.Width>0)
            {
                c.width = column.Width;
            }
            if(column.MaxWidth>0)
            {
                c.maxWidth = column.MaxWidth;
            }
            if(column.MinWidth >0)
            {
                c.minWidth = column.MinWidth;
            }

            return c;
        }
    }

    public class GridViewHeader
    {
        public static MultiColumnHeader CreateTreeViewHeader(string[] titles,string[] tooltips = null)
        {
            Column[] columns = new Column[titles.Length];
            for(int i =0;i<titles.Length;++i)
            {
                columns[i] = new GridViewColumn(titles[i], tooltips != null ? tooltips[i] : string.Empty);
            }

            return new MultiColumnHeader(new MultiColumnHeaderState(columns));
        }

        public static MultiColumnHeader CreateTreeViewHeader(GridViewColumn[] values)
        {
            Column[] columns = new Column[values.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                columns[i] = values[i];
            }

            return new MultiColumnHeader(new MultiColumnHeaderState(columns));
        }

    }
}
