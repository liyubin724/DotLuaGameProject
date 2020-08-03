using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using SystemObject = System.Object;

namespace DotEditor.UI
{
    public static class SpriteAtlasExporter
    {
        public static string[] Export(SpriteAtlas atlas,string dirPath)
        {
            string platformName = "Standalone";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                platformName = "Android";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                platformName = "iPhone";
            }

            TextureImporterPlatformSettings tips = atlas.GetPlatformSettings(platformName);
            TextureImporterPlatformSettings cachedTips = new TextureImporterPlatformSettings();
            tips.CopyTo(cachedTips);

            tips.overridden = true;
            tips.format = TextureImporterFormat.RGBA32;
            atlas.SetPlatformSettings(tips);

            List<string> texturePathList = new List<string>();

            SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { atlas}, EditorUserBuildSettings.activeBuildTarget);
            MethodInfo getPreviewTextureMI = typeof(SpriteAtlasExtensions).GetMethod("GetPreviewTextures", BindingFlags.Static | BindingFlags.NonPublic);
            Texture2D[] atlasTextures = (Texture2D[])getPreviewTextureMI.Invoke(null, new SystemObject[] { atlas });
            if (atlasTextures != null && atlasTextures.Length > 0)
            {
                for (int i = 0; i < atlasTextures.Length; i++)
                {
                    Texture2D packTexture = atlasTextures[i];
                    byte[] rawBytes = packTexture.GetRawTextureData();

                    Texture2D nTexture = new Texture2D(packTexture.width, packTexture.height, packTexture.format, false, false);
                    nTexture.LoadRawTextureData(rawBytes);
                    nTexture.Apply();
                    string textPath = string.Format("{0}/{1}_{2}.png", dirPath, atlas.name, i);
                    File.WriteAllBytes(textPath, nTexture.EncodeToPNG());

                    texturePathList.Add(textPath);
                }
            }

            atlas.SetPlatformSettings(cachedTips);

            return texturePathList.ToArray();
        }


        [MenuItem("Game/UI/SpriteAtlas Exporter")]
        private static void ExportAltas()
        {
            List<SpriteAtlas> atlasList = new List<SpriteAtlas>();
            if(Selection.objects!=null && Selection.objects.Length>0)
            {
                foreach(var obj in Selection.objects)
                {
                    if(obj.GetType() == typeof(SpriteAtlas))
                    {
                        atlasList.Add(obj as SpriteAtlas);
                    }
                }
            }

            if(atlasList.Count==0)
            {
                EditorUtility.DisplayDialog("Tips", "Please Selected SpriteAtlas", "OK");
                return;
            }

            string dirPath = EditorUtility.OpenFolderPanel("Save Dir", "D:/", "");
            if(string.IsNullOrEmpty(dirPath))
            {
                EditorUtility.DisplayDialog("Tips", "Please Selected a folder","OK");
                return;
            }

            foreach(var atlas in atlasList)
            {
                Export(atlas, dirPath);
            }
        }
    }
}
