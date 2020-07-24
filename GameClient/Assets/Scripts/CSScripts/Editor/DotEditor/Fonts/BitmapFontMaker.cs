using DotEditor.GUIExtension;
using DotEditor.GUIExtension.ListView;
using DotEditor.NativeDrawer;
using DotEditor.Utilities;
using DotEngine.Fonts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static DotEditor.Fonts.BitmapFontConfig;
using static DotEngine.Fonts.BitmapFont;

namespace DotEditor.Fonts
{
    public class BitmapFontMaker : EditorWindow
    {
        private static string MATERIAL_SHADER = "Unlit/Transparent";

        private static float TOOLBAR_HEIGHT = 20;

        private static float LISTVIEW_DRAG_WIDTH = 5;
        private static float MIN_LISTVIEW_WIDTH = 100;
        private static float MAX_LISTVIEW_WIDTH = 300;

        [MenuItem("Game/Bitmap Font Maker")]
        public static void ShowWin()
        {
            var win = GetWindow<BitmapFontMaker>();
            win.titleContent = Contents.titleContent;
            win.minSize = new Vector2(400, 300);
            win.Show();
        }

        private List<BitmapFontConfig> fontConfigs = new List<BitmapFontConfig>();
        private SimpleListView<BitmapFontConfig> fontConfigListView = null;

        private float listViewWidth = 120;
        private int charIDStart = 19968;
        
        void Awake()
        {
            BitmapFontConfig[] datas = AssetDatabaseUtility.FindInstances<BitmapFontConfig>();
            if(datas!=null && datas.Length >0)
            {
                fontConfigs.AddRange(datas);
            }
        }

        private int selectedIndex = -1;
        private NativeDrawerObject drawerObject = null;
        private void SelectedChanged(int index)
        {
            if(selectedIndex != index)
            {
                selectedIndex = index;

                fontConfigListView.SetSelection(selectedIndex);
                if(selectedIndex>=0)
                {
                    drawerObject = new NativeDrawerObject(fontConfigs[selectedIndex])
                    {
                        IsShowScroll = true,
                    };
                }
                else
                {
                    drawerObject = null;
                }
            }
        }

        void OnGUI()
        {
            if(fontConfigListView == null)
            {
                fontConfigListView = new SimpleListView<BitmapFontConfig>();
                fontConfigListView.AddItems(fontConfigs.ToArray());
                fontConfigListView.OnSelectedChange = (index) =>
                {
                    SelectedChanged(index);
                };
            }

            Rect toolbarRect = new Rect(0, 0, position.width, TOOLBAR_HEIGHT);
            DrawToolbar(toolbarRect);

            Rect listViewRect = new Rect(0, TOOLBAR_HEIGHT, listViewWidth, position.height - TOOLBAR_HEIGHT);
            DrawListView(listViewRect);

            EGUI.DrawAreaLine(listViewRect, Color.grey);

            Rect listViewDragRect = new Rect(listViewRect.x + listViewRect.width, listViewRect.y, LISTVIEW_DRAG_WIDTH, listViewRect.height);
            DrawListViewDrag(listViewDragRect);

            EGUI.DrawAreaLine(listViewDragRect, Color.grey);

            Rect contentRect = new Rect(listViewDragRect.x + listViewDragRect.width, listViewRect.y, position.width - listViewDragRect.x - listViewDragRect.width, listViewRect.height);
            DrawContent(contentRect);

            EGUI.DrawAreaLine(contentRect, Color.grey);
        }

        private void DrawToolbar(Rect rect)
        {
            EditorGUI.LabelField(rect, GUIContent.none,EditorStyles.toolbar);
            
            Rect createRect = rect;
            createRect.width = Styles.toolbarWidth;
            if(GUI.Button(createRect,Contents.creatContent, EditorStyles.toolbarButton))
            {
                string filePath = EditorUtility.SaveFilePanel("Create Font Config", Application.dataPath, "bitmap_font_config", "asset");
                if(!string.IsNullOrEmpty(filePath))
                {
                    BitmapFontConfig config = ScriptableObject.CreateInstance<BitmapFontConfig>();
                    config.name = Path.GetFileNameWithoutExtension(filePath);

                    string fileAssetPath = PathUtility.GetAssetPath(filePath);
                    AssetDatabase.CreateAsset(config, fileAssetPath);
                    AssetDatabase.ImportAsset(fileAssetPath);
                    
                    fontConfigs.Add(config);
                    fontConfigListView.AddItem(config);
                    SelectedChanged(fontConfigs.Count - 1);
                }
            }

            Rect deleteRect = createRect;
            deleteRect.x += createRect.width;
            EditorGUI.BeginDisabledGroup(selectedIndex < 0);
            {
                if(GUI.Button(deleteRect,Contents.deleteContent, EditorStyles.toolbarButton))
                {
                    if(selectedIndex>0)
                    {
                        BitmapFontConfig font = fontConfigs[selectedIndex];
                        AssetDatabase.DeleteAsset(font.GetFontDataPath());
                        AssetDatabase.DeleteAsset(font.GetFontPath());
                        AssetDatabase.DeleteAsset(font.GetFontTexturePath());

                        fontConfigs.RemoveAt(selectedIndex);
                        fontConfigListView.RemoveAt(selectedIndex);

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(font));

                        SelectedChanged(-1);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();

            Rect exportRect = deleteRect;
            exportRect.x += deleteRect.width;
            EditorGUI.BeginDisabledGroup(selectedIndex < 0);
            {
                if (GUI.Button(exportRect,Contents.exportContent, EditorStyles.toolbarButton))
                {
                    ExportFont(fontConfigs[selectedIndex]);
                    AssetDatabase.SaveAssets();
                }
            }
            EditorGUI.EndDisabledGroup();

            Rect exportAllRect = rect;
            exportAllRect.x += rect.width - Styles.toolbarWidth;
            exportAllRect.width = Styles.toolbarWidth;
            if(GUI.Button(exportAllRect,Contents.exportAllContent, EditorStyles.toolbarButton))
            {
                foreach(var config in fontConfigs)
                {
                    ExportFont(config);
                }
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawListView(Rect rect)
        {
            if(fontConfigListView == null)
            {
                fontConfigListView = new SimpleListView<BitmapFontConfig>();
                fontConfigListView.AddItems(fontConfigs.ToArray());
            }

            fontConfigListView.OnGUI(rect);
        }

        private bool isListViewDragging = false;
        private void DrawListViewDrag(Rect dragRect)
        {
            EGUI.DrawVerticalLine(dragRect, Color.grey, 2.0f);

            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && dragRect.Contains(Event.current.mousePosition))
                {
                    isListViewDragging = true;

                    Event.current.Use();
                    Repaint();
                }
                else if (isListViewDragging && Event.current.type == EventType.MouseDrag)
                {
                    listViewWidth += Event.current.delta.x;
                    if (listViewWidth < MIN_LISTVIEW_WIDTH)
                    {
                        listViewWidth = MIN_LISTVIEW_WIDTH;
                    }
                    else if (listViewWidth > MAX_LISTVIEW_WIDTH)
                    {
                        listViewWidth = MAX_LISTVIEW_WIDTH;
                    }
                    Repaint();
                }
                else if (isListViewDragging && Event.current.type == EventType.MouseUp)
                {
                    isListViewDragging = false;
                    Event.current.Use();
                    Repaint();
                }
            }
        }

        private void DrawContent(Rect rect)
        {
            if(selectedIndex<0)
            {
                return;
            }

            if(drawerObject != null)
            {
                GUILayout.BeginArea(rect);
                {
                    drawerObject.OnGUILayout();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(fontConfigs[selectedIndex]);
                    }
                }
                GUILayout.EndArea();
            }
        }

        private void ExportFont(BitmapFontConfig config)
        {
            if(!config.IsValid())
            {
                Debug.LogError("FontConfig is unvalid");
                return;
            }

            int charIndex = charIDStart;
            List<Texture2D> textures = new List<Texture2D>();
            for(int i =0;i<config.fontChars.Count;++i)
            {
                BitmapFontChar fontChar = config.fontChars[i];
                fontChar.charIndexes = new int[fontChar.chars.Count];
                for(int j = 0;j<fontChar.chars.Count;++j)
                {
                    fontChar.charIndexes[j] = (charIndex++);
                    
                    textures.Add(fontChar.textures[j]);

                    SetCharTextureSetting(fontChar.textures[j]);
                }
            }

            Texture2D atlas = PackCharTexture(config.GetFontTexturePath(), textures.ToArray(), config.padding, config.maxSize, out Rect[] rects);
            if(atlas == null)
            {
                Debug.LogError("PackCharTexture failed");
                return;
            }
            int rectIndex = 0;
            for (int i = 0; i < config.fontChars.Count; ++i)
            {
                BitmapFontChar fontChar = config.fontChars[i];
                fontChar.charRects = new Rect[fontChar.chars.Count];
                for (int j = 0; j < fontChar.chars.Count; ++j)
                {
                    fontChar.charRects[j] = rects[rectIndex];
                    ++rectIndex;
                }
            }

            Font font = CreateFont(config, atlas, out BitmapFontCharMap[] charMaps);

            string fontDataPath = config.GetFontDataPath();
            BitmapFont fontData = AssetDatabase.LoadAssetAtPath<BitmapFont>(fontDataPath);
            if(fontData == null)
            {
                fontData = ScriptableObject.CreateInstance<BitmapFont>();
                AssetDatabase.CreateAsset(fontData, fontDataPath);
                AssetDatabase.ImportAsset(fontDataPath);
            }

            fontData.bmFont = font;
            fontData.charMaps = charMaps;

            EditorUtility.SetDirty(fontData);
        }

        private Font CreateFont(BitmapFontConfig config, Texture2D atlas,out BitmapFontCharMap[] charMap)
        {
            string fontAssetPath = config.GetFontPath();
            charMap = new BitmapFontCharMap[config.fontChars.Count];

            Font font = AssetDatabase.LoadMainAssetAtPath(fontAssetPath) as Font;
            if(font == null)
            {
                font = new Font();
                AssetDatabase.CreateAsset(font, fontAssetPath);
                AssetDatabase.ImportAsset(fontAssetPath);
            }
            Shader matShader = Shader.Find(MATERIAL_SHADER);

            Material fontMat = AssetDatabase.LoadAssetAtPath<Material>(fontAssetPath);
            if(fontMat == null)
            {
                fontMat = new Material(matShader);
                fontMat.name = "Font Material";
                AssetDatabase.AddObjectToAsset(fontMat, fontAssetPath);
                
                font.material = fontMat;
            }

            fontMat.mainTexture = atlas;

            List<CharacterInfo> charInfos = new List<CharacterInfo>();
            for (int i = 0; i < config.fontChars.Count; ++i)
            {
                BitmapFontChar fontChar = config.fontChars[i];
                charMap[i] = new BitmapFontCharMap()
                {
                    name = fontChar.fontName,
                    orgChars = fontChar.chars.ToArray(),
                    mapChars = (from index in fontChar.charIndexes select (char)index).ToArray(),
                };

                for (int j = 0; j < fontChar.chars.Count; ++j)
                {
                    CharacterInfo cInfo = new CharacterInfo();
                    cInfo.index = fontChar.charIndexes[j];
                    Rect rect = fontChar.charRects[j];

                    cInfo.uvBottomLeft = new Vector2(rect.x, rect.y);
                    cInfo.uvTopRight = new Vector2(rect.x + rect.width, rect.y + rect.height);
                    cInfo.uvBottomRight = new Vector2(rect.x + rect.width, rect.y);
                    cInfo.uvTopLeft = new Vector2(rect.x, rect.y + rect.height);

                    Rect vert = new Rect(0, -rect.height * atlas.height, rect.width * atlas.width, rect.height * atlas.height);
                    cInfo.minX = (int)vert.xMin;
                    cInfo.maxX = (int)vert.xMax;
                    cInfo.minY = (int)vert.yMin;
                    cInfo.maxY = -(int)vert.yMax;

                    cInfo.glyphWidth = Mathf.RoundToInt(atlas.width * rect.width);
                    cInfo.glyphHeight = Mathf.RoundToInt(atlas.height * rect.height);

                    cInfo.advance = cInfo.glyphWidth + fontChar.charSpace;
                    cInfo.bearing = 0;

                    cInfo.style = FontStyle.Normal;

                    charInfos.Add(cInfo);
                }
            }

            font.characterInfo = charInfos.ToArray();

            EditorUtility.SetDirty(font);
            return font;
        }

        private void SetCharTextureSetting(Texture2D texture)
        {
            string texAssetPath = AssetDatabase.GetAssetPath(texture);

            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(texAssetPath);
            TextureImporterSettings tis = new TextureImporterSettings();
            ti.ReadTextureSettings(tis);
            tis.textureType = TextureImporterType.Default;
            tis.textureShape = TextureImporterShape.Texture2D;
            tis.sRGBTexture = true;
            tis.alphaSource = TextureImporterAlphaSource.FromInput;
            tis.alphaIsTransparency = true;
            tis.mipmapEnabled = false;
            tis.readable = true;
            tis.npotScale = TextureImporterNPOTScale.None;
            ti.SetTextureSettings(tis);

            TextureImporterPlatformSettings ps = ti.GetPlatformTextureSettings("Standalone");
            ps.overridden = true;
            ps.format = TextureImporterFormat.RGBA32;
            ti.SetPlatformTextureSettings(ps);

            AssetDatabase.WriteImportSettingsIfDirty(texAssetPath);
        }

        private Texture2D PackCharTexture(string atlasAssetPath,Texture2D[] textures, int padding,int maxSize ,out Rect[] rects)
        {
            Texture2D fontAtlas = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            rects = null;
            if (textures != null && textures.Length > 0)
            {
                rects = fontAtlas.PackTextures(textures.ToArray(), padding, maxSize);
            }
            string fontAtlasDiskPath = PathUtility.GetDiskPath(atlasAssetPath);
            File.WriteAllBytes(fontAtlasDiskPath, fontAtlas.EncodeToPNG());
            AssetDatabase.ImportAsset(atlasAssetPath);

            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(atlasAssetPath);
            TextureImporterSettings tis = new TextureImporterSettings();
            ti.ReadTextureSettings(tis);
            tis.textureType = TextureImporterType.Default;
            tis.textureShape = TextureImporterShape.Texture2D;
            tis.sRGBTexture = true;
            tis.alphaSource = TextureImporterAlphaSource.FromInput;
            tis.alphaIsTransparency = true;
            tis.mipmapEnabled = false;
            tis.readable = false;
            tis.npotScale = TextureImporterNPOTScale.None;
            ti.SetTextureSettings(tis);

            TextureImporterPlatformSettings ps = ti.GetPlatformTextureSettings("Standalone");
            ps.overridden = true;
            ps.format = TextureImporterFormat.RGBA32;
            ti.SetPlatformTextureSettings(ps);

            ps = ti.GetPlatformTextureSettings("Android");
            ps.overridden = true;
            ps.format = TextureImporterFormat.ETC2_RGBA8;
            ti.SetPlatformTextureSettings(ps);

            //ps = ti.GetPlatformTextureSettings("iPhone");
            //ps.overridden = true;
            //ps.format = TextureImporterFormat.ASTC_RGBA_4x4;
            //ti.SetPlatformTextureSettings(ps);

            AssetDatabase.WriteImportSettingsIfDirty(atlasAssetPath);

            fontAtlas = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasAssetPath);
            return fontAtlas;
        }

        class Contents
        {
            public static GUIContent titleContent = new GUIContent("Bitmap Font Maker");
            public static GUIContent creatContent = new GUIContent("Create", "create a new data");
            public static GUIContent deleteContent = new GUIContent("Delete", "delete the data which be selected");
            public static GUIContent exportContent = new GUIContent("Export", "export the data");
            public static GUIContent exportAllContent = new GUIContent("Export All", "export all");
        }

        class Styles
        {
            public static float toolbarWidth = 60;
            public static float singleLineHeight = EditorGUIUtility.singleLineHeight;
            public static float singleTitleLineHeight = 20;

        }
    }
}
