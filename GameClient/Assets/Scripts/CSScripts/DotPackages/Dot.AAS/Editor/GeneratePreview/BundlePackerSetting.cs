namespace DotEditor.AAS
{
    public enum GenerateBuildTarget
    {
        StandaloneWindows = 5,
        iOS = 9,
        Android = 13,
    }

    public class BundlePackerSetting
    {
        public GenerateBuildTarget target = GenerateBuildTarget.StandaloneWindows;
        public string tempOutputFolder = string.Empty;
        public string outputFolder = string.Empty;
        
        public bool usedMd5ForBundle = false;
        public string bundleDetailFileName = string.Empty;
    }
}
