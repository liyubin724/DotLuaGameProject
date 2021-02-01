namespace DotEditor.Utilities
{
    public static class ExplorerUtility
    {
        public static void OpenExplorerFolder(string dirPath)
        {
#if UNITY_EDITOR
            System.Diagnostics.Process open = new System.Diagnostics.Process();
            open.StartInfo.FileName = "explorer";
            open.StartInfo.Arguments = @"/e /root," + dirPath.Replace("/", "\\");
            open.Start();
#endif
        }

        public static void OpenExplorerFile(string filePath)
        {
#if UNITY_EDITOR
            System.Diagnostics.Process open = new System.Diagnostics.Process();
            open.StartInfo.FileName = "explorer";
            open.StartInfo.Arguments = @"/select," + filePath.Replace("/", "\\");
            open.Start();
#endif
        }
    }
}
