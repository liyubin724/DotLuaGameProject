namespace DotEditor.Config.WDB
{
    public class ExcelStyle
    {
        public string MarkFlag { get; set; } = "#ETD#";

        public int RowStartIndex { get; set; } = 0;
        public int ColumnStartIndex { get; set; } = 0;
        public int FieldRowCount { get; set; } = 6;
        public string LineStartFlag { get; set; } = "start";
        public string LineEndFlag { get; set; } = "end";

        public int ColumnMinCount { get; set; } = 2;

        public static ExcelStyle DefaultStyle = new ExcelStyle();
    }
}
