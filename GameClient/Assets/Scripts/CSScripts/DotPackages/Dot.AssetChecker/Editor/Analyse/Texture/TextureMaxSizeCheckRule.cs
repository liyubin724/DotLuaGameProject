using UnityEngine;

namespace DotEditor.AssetChecker
{
    [AnalyseRule("Texture","Max Size")]
    public class TextureMaxSizeCheckRule : TextureAnalyseRuler
    {
        public int maxWidth = 1024;
        public int maxHeight = 1024;

        public override bool AnalyseTexture(Texture texture, ref int errorCode)
        {
            if(texture.width>maxWidth)
            {
                errorCode = ResultCode.ERR_CHECK_TEXTURE_LT_MAX_WIDTH;
                return false;
            }
            if(texture.height>maxHeight)
            {
                errorCode = ResultCode.ERR_CHECK_TEXTURE_LT_MAX_HEIGHT;
                return false;
            }
            return true;
        }
    }
}
