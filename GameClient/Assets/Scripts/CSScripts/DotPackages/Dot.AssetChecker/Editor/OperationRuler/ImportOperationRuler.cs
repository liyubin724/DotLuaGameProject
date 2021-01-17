using UnityEditor;

namespace DotEditor.AssetChecker
{
    public abstract class ImportOperationRuler : IOperationRuler
    {
        public bool Enable { get; set; } = true;

        public bool Run(string assetPath, ref int errorCode)
        {
            if(!Enable)
            {
                return true;
            }

            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer == null)
            {
                errorCode = ResultCode.ERR_OPER_ASSET_IMPORT_INVALID;
                return false;
            }

            Import(importer);

            return true;
        }

        protected abstract void Import(AssetImporter importer);
    }
}
