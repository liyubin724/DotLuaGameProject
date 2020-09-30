using UnityEngine;
using static UnityEditor.IMGUI.Controls.MultiColumnHeaderState;

namespace DotEditor.GUIExtension.GridGUI
{
    public class GridColumn
    {
        public string Title { get; private set; } = string.Empty;
        public string Tooltip { get; private set; } = string.Empty;
        public bool AutoResize { get; set; } = true;
        public int MaxWidth { get; set; } = int.MaxValue;
        public int MinWidth { get; set; } = 40;

        public GridColumn(string title,string tooltip)
        {
            Title = title;
            Tooltip = tooltip;
        }

        public static implicit operator Column(GridColumn column)
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
}
