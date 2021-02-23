using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AAS.Reprocessor
{
    public abstract class AAssetImporterReprocess : IAssetReprocess
    {
        public void Execute(string assetPath)
        {
            UnityObject uObj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
            if(uObj == null || !IsValid(uObj))
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                SetImporter(importer);
            }
            return;
        }

        protected abstract bool IsValid(UnityObject uObj);
        protected abstract void SetImporter(AssetImporter importer);
    }
}
