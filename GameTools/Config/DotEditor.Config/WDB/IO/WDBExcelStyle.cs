using DotEngine.Config.WDB;

namespace DotEditor.Config.WDB
{
    public class WDBExcelStyle
    {
        public string MarkFlag { get; set; } = "#ETD#";

        public int RowStartIndex { get; set; } = 0;
        public int ColumnStartIndex { get; set; } = 0;
        public int FieldRowCount { get; set; } = 6;
        public string LineStartFlag { get; set; } = "start";
        public string LineEndFlag { get; set; } = "end";

        public WDBFieldPlatform TargetPlatform { get; set; } = WDBFieldPlatform.Client | WDBFieldPlatform.Server;

        public int ColumnMinCount { get; set; } = 2;

        public static WDBExcelStyle DefaultStyle = new WDBExcelStyle();
    }
}
