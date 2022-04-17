using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.Assets.Post.Rulers
{
    [CustomEditor(typeof(SetAtlasPlatformRuler))]
    public class SetAtlasPlatformRulerEditor : Editor
    {
        private string[] PlatformContents = new string[]
        {
            "Standalone",
            "Android",
            "iPhone",
        };

        private int[] MaxSizeValues = new int[]
        {
            128,
            256,
            1024,
            2048,
            4096
        };
        private string[] MaxSizeContents = new string[]
        {
            "128",
            "256",
            "1024",
            "2048",
            "4096"
        };

        private int[] StandaloneFormatValues = new int[]
        {
            (int)TextureImporterFormat.DXT5,
            (int)TextureImporterFormat.RGBA16,
            (int)TextureImporterFormat.RGBA32,
        };

        private string[] StandaloneFormatContents = new string[]
        {
            "DXT5",
            "RGBA16",
            "RGBA32",
        };

        private int[] AndroidFormatValues = new int[]
        {
            (int)TextureImporterFormat.ETC2_RGBA8,
            (int)TextureImporterFormat.ETC2_RGBA8Crunched,
            (int)TextureImporterFormat.RGBA16,
            (int)TextureImporterFormat.RGBA32,
        };

        private string[] AndroidFormatContents = new string[]
        {
            "ETC2_RGBA8",
            "ETC2_RGBA8Crunched",
            "RGBA16",
            "RGBA32",
        };

        private int[] iPhoneFormatValues = new int[]
        {
            (int)TextureImporterFormat.ASTC_4x4,
            (int)TextureImporterFormat.ASTC_8x8,
            (int)TextureImporterFormat.RGBA16,
            (int)TextureImporterFormat.RGBA32,
        };

        private string[] iPhoneFormatContents = new string[]
        {
            "ASTC_4x4",
            "ASTC_8x8",
            "RGBA16",
            "RGBA32"
        };

        private SetAtlasPlatformRuler ruler = null;
        private void OnEnable()
        {
            ruler = target as SetAtlasPlatformRuler;
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            ruler.Platform = EGUILayout.StringPopup("Platform", ruler.Platform, PlatformContents);
            ruler.MaxSize = EGUILayout.DrawPopup<int>("MaxSize", MaxSizeContents, MaxSizeValues, ruler.MaxSize);
            if (ruler.Platform == PlatformContents[0])
            {
                ruler.Format = EGUILayout.DrawPopup<int>("Format", StandaloneFormatContents, StandaloneFormatValues, ruler.Format);
            } else if (ruler.Platform == PlatformContents[1])
            {
                ruler.Format = EGUILayout.DrawPopup<int>("Format", AndroidFormatContents, AndroidFormatValues, ruler.Format);
            }
            else if(ruler.Platform == PlatformContents[2])
            {
                ruler.Format = EGUILayout.DrawPopup<int>("Format", iPhoneFormatContents, iPhoneFormatValues, ruler.Format);
            }
        }
    }
}
