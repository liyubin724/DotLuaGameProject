using UnityEditor;

namespace DotEditor.AssetChecker
{
    public abstract class TextureOperationRuler : ImportOperationRuler
    {
        protected override void Import(AssetImporter importer)
        {
            ImportTexture(importer as TextureImporter);
        }

        protected abstract void ImportTexture(TextureImporter textureImporter);
    }
}
