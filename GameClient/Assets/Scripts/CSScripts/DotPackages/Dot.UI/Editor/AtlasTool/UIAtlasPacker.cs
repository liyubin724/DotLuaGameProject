using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityObject = UnityEngine.Object;
using SystemObject = System.Object;
using DotEditor.Utilities;

namespace DotEditor.UI
{
    public class UIAtlasPacker
    {
        private static readonly string UISpriteRootPath = "Assets/ArtRes/UI";
        private static readonly string UIAtlasRootPath = "Assets/ArtRes/UI/Atlas";

        [MenuItem("Game/UI/Pack Atlas", false, 10)]
        private static void ExportAltas()
        {
            if (Selection.objects == null || Selection.objects.Length == 0)
                return;
            string platformName = "Standalone";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                platformName = "Android";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                platformName = "iPhone";
            }
            List<SpriteAtlas> atlasList = new List<SpriteAtlas>();
            Dictionary<SpriteAtlas, TextureImporterPlatformSettings> tipsDic = new Dictionary<SpriteAtlas, TextureImporterPlatformSettings>();
            foreach (UnityObject uObj in Selection.objects)
            {
                SpriteAtlas atlas = uObj as SpriteAtlas;
                if (atlas != null)
                {
                    atlasList.Add(atlas);

                    TextureImporterPlatformSettings tips = atlas.GetPlatformSettings(platformName);
                    TextureImporterPlatformSettings ntips = new TextureImporterPlatformSettings();
                    tips.CopyTo(ntips);
                    tipsDic.Add(atlas, ntips);

                    tips.overridden = true;
                    tips.format = TextureImporterFormat.RGBA32;
                    atlas.SetPlatformSettings(tips);
                }
            }
            if (atlasList.Count == 0)
            {
                EditorUtility.DisplayDialog("Warning", "Can't Found Sprite Atlas In Selection", "OK");
                return;
            }
            UnityEditor.U2D.SpriteAtlasUtility.PackAtlases(atlasList.ToArray(), EditorUserBuildSettings.activeBuildTarget);
            string outputDirPath = EditorUtility.OpenFolderPanel("Save Atlas To", "D:/", "");
            if (string.IsNullOrEmpty(outputDirPath))
            {
                EditorUtility.DisplayDialog("Warning", "Please Choose a Folder", "OK");
                return;
            }

            MethodInfo getPreviewTextureMI = typeof(SpriteAtlasExtensions).GetMethod("GetPreviewTextures", BindingFlags.Static | BindingFlags.NonPublic);
            foreach (SpriteAtlas atlas in atlasList)
            {
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
                        string textPath = string.Format("{0}/{1}_{2}.png", outputDirPath, atlas.name, i);
                        File.WriteAllBytes(textPath, nTexture.EncodeToPNG());
                    }
                }
            }

            foreach (var kvp in tipsDic)
            {
                kvp.Key.SetPlatformSettings(kvp.Value);
            }
        }


        [MenuItem("Game/UI/Atlas Maker &p", false, 11)]
        private static void AtlasMaker()
        {
            string[] guids = Selection.assetGUIDs;
            List<SpriteAtlas> atlasList = new List<SpriteAtlas>();
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string diskPath = PathUtility.GetDiskPath(assetPath);
                string[] allFiles = Directory.GetFiles(diskPath, "*.png", SearchOption.TopDirectoryOnly);

                string[] splitDir = assetPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string packName = splitDir[splitDir.Length - 1];
                List<Sprite> sprites = new List<Sprite>();
                foreach (var f in allFiles)
                {
                    string fileAssetPath = PathUtility.GetAssetPath(f);
                    SetSpriteMetaSetting(fileAssetPath);

                    Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(fileAssetPath);
                    sprites.Add(s);
                }

                var atlas = PackSpritesToAtlas(packName, sprites.ToArray());
                atlasList.Add(atlas);
            }

            Selection.objects = atlasList.ToArray();
        }

        [MenuItem("Game/UI/Atlas Maker &p", true)]
        private static bool ValidateAtlasMaker()
        {
            string[] guids = Selection.assetGUIDs;
            if (guids == null)
            {
                return false;
            }
            bool isValid = true;
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string diskPath = PathUtility.GetDiskPath(assetPath);
                if (!Directory.Exists(diskPath) || !assetPath.StartsWith(UISpriteRootPath))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        private static void SetSpriteMetaSetting(string assetPath)
        {
            TextureImporter texImp = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            texImp.textureType = TextureImporterType.Sprite;
            texImp.spriteImportMode = SpriteImportMode.Single;
            texImp.spritePackingTag = "";
            texImp.spritePixelsPerUnit = 100;
            texImp.sRGBTexture = true;
            texImp.alphaIsTransparency = true;
            texImp.alphaSource = TextureImporterAlphaSource.FromInput;
            texImp.isReadable = false;
            texImp.mipmapEnabled = false;
            texImp.SaveAndReimport();
        }

        private static SpriteAtlas PackSpritesToAtlas(string packName, Sprite[] sprites)
        {
            string atlasAssetPath = UIAtlasRootPath + "/" + packName + "_atlas" + ".spriteatlas";
            SpriteAtlas packAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasAssetPath);
            if (packAtlas == null)
            {
                packAtlas = new SpriteAtlas();
                AssetDatabase.CreateAsset(packAtlas, atlasAssetPath);
            }
            packAtlas.Remove(packAtlas.GetPackables());
            packAtlas.Add(sprites);

            SpriteAtlasTextureSettings sats = packAtlas.GetTextureSettings();
            sats.readable = false;
            sats.sRGB = true;
            sats.generateMipMaps = false;
            sats.filterMode = FilterMode.Bilinear;
            packAtlas.SetTextureSettings(sats);

            SpriteAtlasPackingSettings saps = packAtlas.GetPackingSettings();
            saps.enableRotation = false;
            saps.padding = 2;
            saps.enableTightPacking = false;
            packAtlas.SetPackingSettings(saps);

            TextureImporterPlatformSettings winTips = packAtlas.GetPlatformSettings("Standalone");
            winTips.overridden = true;
            winTips.maxTextureSize = 2048;
            winTips.format = TextureImporterFormat.DXT5;
            packAtlas.SetPlatformSettings(winTips);

            TextureImporterPlatformSettings androidTips = packAtlas.GetPlatformSettings("Android");
            androidTips.maxTextureSize = 2048;
            androidTips.overridden = true;
            androidTips.format = TextureImporterFormat.ETC2_RGBA8;
            packAtlas.SetPlatformSettings(androidTips);

            TextureImporterPlatformSettings iOSTips = packAtlas.GetPlatformSettings("iPhone");
            iOSTips.maxTextureSize = 2048;
            iOSTips.overridden = true;
            iOSTips.format = TextureImporterFormat.ASTC_4x4;
            packAtlas.SetPlatformSettings(iOSTips);

            AssetDatabase.ImportAsset(atlasAssetPath);
            return packAtlas;
        }
    }

}

