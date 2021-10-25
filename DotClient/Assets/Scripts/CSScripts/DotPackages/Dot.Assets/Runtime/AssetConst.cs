namespace DotEngine.Assets
{
    public static class AssetConst
    {
        public static string ASSET_DETAIL_CONFIG_NAME = "asset_detail_config";
        public static string BUNDLE_DETAIL_CONFIG_NAME = "bundle_detail_config";

        public static string GetAssetDetailConfigPathInProject()
        {
            return $"Assets/{ASSET_DETAIL_CONFIG_NAME}.json";
        }

        public static string GetAssetDetailConfigFileName()
        {
            return $"{ASSET_DETAIL_CONFIG_NAME}.json";
        }

        public static string GetBundleDetailConfigFileName()
        {
            return $"{BUNDLE_DETAIL_CONFIG_NAME}.json";
        }
    }
}
