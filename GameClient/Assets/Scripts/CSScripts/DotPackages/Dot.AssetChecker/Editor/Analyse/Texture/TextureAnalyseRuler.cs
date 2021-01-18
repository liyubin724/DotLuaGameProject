using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public abstract class TextureAnalyseRuler : IUnityObjectAnalyseRule
    {
        public bool AnalyseAsset(UnityObject uObj, ref int errorCode)
        {
            if (uObj is Texture texture && texture != null)
            {
                return AnalyseTexture(texture,ref errorCode);
            }
            errorCode = ResultCode.ERR_CHECK_TEXTURE_INVALID;
            return false;
        }

        public abstract bool AnalyseTexture(Texture texture, ref int errorCode);
    }
}
