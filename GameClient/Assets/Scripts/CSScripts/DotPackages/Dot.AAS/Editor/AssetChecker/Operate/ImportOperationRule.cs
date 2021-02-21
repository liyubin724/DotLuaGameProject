using UnityEditor;

namespace DotEditor.AssetChecker
{
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
