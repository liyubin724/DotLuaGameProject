namespace DotTool.Config
{
    public class WDBStyle
    {
        public string MarkFlag { get; set; } = "#ETD#";
        public string LineStartFlag { get; set; } = "start";
        public string LineEndFlag { get; set; } = "end";

        internal int RowStartIndex { get; set; } = 0;
        internal int RowEndIndex { get; set; } = 0;
        internal int RowCount => RowEndIndex - RowStartIndex + 1;

        internal int ColumnStartIndex { get; set; } = 0;
        internal int ColumnEndIndex { get; set; } = 0;
        internal int ColumnCount => ColumnEndIndex - ColumnStartIndex + 1;
    }
}
