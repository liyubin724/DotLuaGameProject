using UnityEditor;

namespace DotEditor.AssetChecker
{
    public enum AssetPlatformType
    {
        Window = 0,
        Android,
        iOS,
    }

    public abstract class ImportOperationRule : OperationRule
    {
        public override void Execute(string assetPath)
        {
            if(!enable)
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                ImportAsset(importer);
            }
        }

        protected abstract void ImportAsset(AssetImporter importer);
    }
}
