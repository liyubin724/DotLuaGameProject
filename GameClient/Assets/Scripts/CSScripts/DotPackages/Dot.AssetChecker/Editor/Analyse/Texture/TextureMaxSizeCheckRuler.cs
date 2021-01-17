using UnityEngine;

namespace DotEditor.AssetChecker
{
    public class TextureMaxSizeCheckRuler : TextureAnalyseRuler
    {
        public int MaxWidth { get; set; } = 1024;
        public int MaxHeight { get; set; } = 1024;

        public override bool AnalyseTexture(Texture texture, ref int errorCode)
        {
            if(texture.width>MaxWidth)
            {
                errorCode = ResultCode.ERR_CHECK_TEXTURE_LT_MAX_WIDTH;
                return false;
            }
            if(texture.height>MaxHeight)
            {
                errorCode = ResultCode.ERR_CHECK_TEXTURE_LT_MAX_HEIGHT;
                return false;
            }
            return true;
        }
    }
}
