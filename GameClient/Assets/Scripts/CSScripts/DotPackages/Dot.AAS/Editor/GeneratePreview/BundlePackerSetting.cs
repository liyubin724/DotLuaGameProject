using DotEngine.AAS;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEditor;

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

        private const string BUNDLE_PACKER_SETTING_FILE = "bundle_packer_setting.json";

        public static BundlePackerSetting GetSetting()
        {
            BundlePackerSetting setting = null;
            string settingFilePath = $"{BundleConst.GetBundleFolderInEditor()}/{BUNDLE_PACKER_SETTING_FILE}";
            if(File.Exists(settingFilePath))
            {
                setting = JsonConvert.DeserializeObject<BundlePackerSetting>(File.ReadAllText(settingFilePath));
            }
            if(setting == null)
            {
                setting = new BundlePackerSetting();
                if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
                {
                    setting.target = GenerateBuildTarget.StandaloneWindows;
                }else if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    setting.target = GenerateBuildTarget.Android;
                }else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                {
                    setting.target = GenerateBuildTarget.iOS;
                }else
                {
                    throw new Exception("Unsported");
                }
                setting.tempOutputFolder = $"{BundleConst.BUNDLE_FOLDER}/{setting.target.ToString()}/Temp";
                setting.outputFolder = $"{BundleConst.BUNDLE_FOLDER}/{setting.target.ToString()}/Output";
            }

            return setting;
        }
    }
}
