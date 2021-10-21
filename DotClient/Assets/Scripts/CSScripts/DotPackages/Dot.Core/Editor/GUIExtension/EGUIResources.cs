using System.IO;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExtension
{
    public static class EGUIResources
    {
        #region 灰色系
        public static Color32 lightGray = new Color32(211, 211, 211,255);
        public static Color32 silver = new Color32(192, 192, 192, 255);
        public static Color32 darkGray = new Color32(169, 169, 169, 255);
        public static Color32 gray = new Color32(128, 128, 128, 255);
        public static Color32 dimGray = new Color32(105, 105, 105, 255);
        #endregion
        public static Color32 dodgerBlue = new Color32(30, 144, 255, 255);
        public static Color32 cornflowerBlue = new Color32(100, 149, 225, 255);

        public static T GetResource<T>(string relativePath) where T: UnityObject
        {
            if(string.IsNullOrEmpty(AssetRelativePath))
            {
                return default;
            }

            string assetPath = $"{AssetRelativePath}/{relativePath}";
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        private static Color32 gridLineColor = new Color(0.45f, 0.45f, 0.45f);
        private static Color32 gridBgColor = new Color(0.18f, 0.18f, 0.18f);

        private static Texture2D gridTexture = null;
        public static Texture2D GridTexture
        {
            get
            {
                if (gridTexture == null)
                {
                    gridTexture = GenerateGridTexture(gridLineColor, gridBgColor);
                }
                return gridTexture;
            }
        }

        private static Texture2D crossTexture = null;
        public static Texture2D CrossTexture
        {
            get
            {
                if (crossTexture == null)
                {
                    crossTexture = GenerateCrossTexture(gridLineColor);
                }
                return crossTexture;
            }
        }

        public static Texture2D GenerateGridTexture(Color line, Color bg)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = bg;
                    if (y % 16 == 0 || x % 16 == 0) col = Color.Lerp(line, bg, 0.65f);
                    if (y == 63 || x == 63) col = Color.Lerp(line, bg, 0.35f);
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }

        public static Texture2D GenerateCrossTexture(Color line)
        {
            Texture2D tex = new Texture2D(64, 64);
            Color[] cols = new Color[64 * 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Color col = line;
                    if (y != 31 && x != 31) col.a = 0;
                    cols[(y * 64) + x] = col;
                }
            }
            tex.SetPixels(cols);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.name = "Grid";
            tex.Apply();
            return tex;
        }

        private static Texture2D scriptIconTexture = null;
        public static Texture2D ScriptIconTexture
        {
            get
            {
                if (scriptIconTexture == null)
                {
                    scriptIconTexture = (EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);
                }
                return scriptIconTexture;
            }
        }

        public static Color BorderColor
        {
            get
            {
                return EditorGUIUtility.isProSkin ? new Color(0.13f, 0.13f, 0.13f) : new Color(0.51f, 0.51f, 0.51f);
            }
        }

        public static Color BackgroundColor
        {
            get
            {
                return EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.83f, 0.83f, 0.83f);
            }
        }

        private static Texture2D defaultFolderIcon = null;
        public static Texture2D DefaultFolderIcon
        {
            get
            {
                if (defaultFolderIcon == null)
                {
                    defaultFolderIcon = EditorGUIUtility.FindTexture("Folder Icon");
                }
                return defaultFolderIcon;
            }
        }

        private static Texture2D infoIcon = null;
        public static Texture2D InfoIcon
        {
            get
            {
                if(infoIcon == null)
                {
                    infoIcon = EditorGUIUtility.FindTexture("console.infoicon");
                }
                return infoIcon;
            }
        }

        private static Texture2D warningIcon = null;
        public static Texture2D WarningIcon
        {
            get
            {
                if(warningIcon ==null)
                {
                    warningIcon = EditorGUIUtility.FindTexture("console.warnicon");
                }
                return warningIcon;
            }
        }

        private static Texture2D errorIcon = null;
        public static Texture2D ErrorIcon
        {
            get
            {
                if (errorIcon == null)
                {
                    errorIcon = EditorGUIUtility.FindTexture("console.erroricon");
                }
                return errorIcon;
            }
        }

        public static Texture2D GetAssetPreview(UnityObject uObject)
        {
            if(uObject == null)
            {
                return ErrorIcon;
            }
            Texture2D previewIcon = AssetPreview.GetAssetPreview(uObject);
            if(previewIcon == null)
            {
                previewIcon = AssetPreview.GetMiniThumbnail(uObject);
            }
            if(previewIcon == null)
            {
                previewIcon = AssetPreview.GetMiniTypeThumbnail(uObject.GetType());
            }
            if(previewIcon == null)
            {
                previewIcon = EGUIResources.MakeColorTexture(80, 80, Color.grey);
            }
            return previewIcon;
        }

        public static Texture2D GetAssetPreview(string assetPath)
        {
            UnityObject uObject = AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath);
            return GetAssetPreview(uObject);
        }

        public static Texture2D GetAssetMiniThumbnail(string assetPath)
        {
            UnityObject uObj = AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath);
            return AssetPreview.GetMiniThumbnail(uObj);
        }

        public static Texture2D MakeColorTexture(int width,int height,Color color)
        {
            Color[] pixel = new Color[width * height];
            for(int i =0;i<pixel.Length;++i)
            {
                pixel[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixel);
            texture.Apply();

            return texture;
        }

        public static GUIContent MakeContent(string text,string tooltip = "",string icon = "")
        {
            GUIContent content;
            if(!string.IsNullOrEmpty(icon))
            {
                content = EditorGUIUtility.IconContent(icon);
            }else
            {
                content = new GUIContent();
            }

            content.text = text;
            content.tooltip = tooltip;

            return content;
        }

        private static string EGUI_ROOT_FOLDER_NAME = "Dot EGUI";
        private static string EGUI_ASSET_FOLDER_NAME = "Editor Resources";

        internal static string RootPath { get; private set; }
        internal static string AssetPath { get; private set; }
        internal static string RootRelativePath { get; private set; }
        internal static string AssetRelativePath { get; private set; }

        [InitializeOnLoadMethod]
        private static void Init()
        {
            var dirs = Directory.GetDirectories(Application.dataPath, EGUI_ROOT_FOLDER_NAME, SearchOption.AllDirectories);
            if (dirs == null || dirs.Length == 0)
            {
                return;
            }

            RootPath = dirs[0].Replace('\\', '/');
            RootRelativePath = RootPath.Replace(Application.dataPath, "Assets");

            AssetPath = RootPath + "/" + EGUI_ASSET_FOLDER_NAME;
            AssetRelativePath = AssetPath.Replace(Application.dataPath, "Assets");
        }
    }
}
