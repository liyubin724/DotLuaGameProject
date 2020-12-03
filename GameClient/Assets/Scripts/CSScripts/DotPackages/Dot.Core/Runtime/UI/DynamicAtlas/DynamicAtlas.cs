/*
 	Based on the Public Domain MaxRectsBinPack.cs source by Sven Magnus
	http://wiki.unity3d.com/index.php/MaxRectsBinPack
 
 	This wrapper by Yuri Beketov
 	for any questions email me beketovman@gmail.com
 	
	This wrapper also public domain.

	DynamicAtlas v.1.0
*/
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HeuristicMethod = DotEngine.UI.DynAtlas.MaxRectsBinPack.FreeRectChoiceHeuristic;

namespace DotEngine.UI.DynAtlas
{
    [System.Serializable]
    public class DynamicAtlas
    {
        /// <summary>
        /// Gets main texture atlas.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the main texture atlas name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return Texture.name; } }

        /// <summary>
        /// Gets the main texture atlas size.
        /// </summary>
        /// <value>The rect.</value>
        public Rect Rect { get { return new Rect(0, 0, Texture.width, Texture.height); } }

        /// <summary>
        /// Gets a value indicating whether this atlas is applied.
        /// </summary>
        /// <value><c>true</c> if this atlas is applied; otherwise, <c>false</c>.</value>
        public bool IsApplied { get; private set; }

        /// <summary>
        /// Gets the pack method.
        /// </summary>
        /// <value>The method.</value>
        public HeuristicMethod Method { get { return method; } }

        /// <summary>
        /// Gets occupancy in the atlas.
        /// </summary>
        /// <value>The occupancy.</value>
        public float Occupancy { get { return rectsPack.Occupancy(); } }

        /// <summary>
        /// Gets the lenght of resources in the atlas.
        /// </summary>
        /// <value>The lenght.</value>
        public int Lenght { get { return rectsPack.usedRectangles.Count; } }

        [SerializeField]
        HeuristicMethod method;
        [SerializeField]
        MaxRectsBinPack rectsPack;
        [SerializeField]
        List<string> names;
        [SerializeField]
        int padding = 2;

        Color transparentColor = new Color(0, 0, 0, 0);

        public DynamicAtlas(string name, int size) : this(width: size, height: size, name: name)
        {
        }

        public DynamicAtlas(string name, int width, int height, int padding = 2, TextureFormat format = TextureFormat.RGBA32, HeuristicMethod method = HeuristicMethod.RectBestShortSideFit)
        {
            Texture = new Texture2D(width, height, format, false,false);
            Texture.name = name;
            this.method = method;
            this.padding = padding;

            rectsPack = new MaxRectsBinPack(width, height, false);
            names = new List<string>();
        }

        #region Write

        /// <summary>
        /// Actually apply all previous Insert changes.
        /// </summary>
        public void Apply()
        {
            Texture.Apply();
            IsApplied = true;
        }

        public bool Write(Texture2D source,string name = null)
        {
            name = name ?? source.name;
            if(names.Contains(name))
            {
                return false;
            }

            Rect newRect = rectsPack.Insert(source.width + 2*padding, source.height + 2*padding, method);

            if (newRect.height == 0)
                return false;

            names.Add(name);

            int x = (int)newRect.x;
            int y = (int)newRect.y;
            int width = (int)newRect.width;
            int height = (int)newRect.height;

            for(int i =0;i<width;++i)
            {
                for(int j = 0;j<padding;++j)
                {
                    Texture.SetPixel(x+i, y+j, transparentColor);
                    Texture.SetPixel(x + i, y + j + height - padding, transparentColor);
                }
            }

            for (int i = 0; i < padding; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                   Texture.SetPixel(x + i, y + j, transparentColor);
                    Texture.SetPixel(x + width - padding + i, y + j, transparentColor);
                }
            }

            Color[] colors = source.GetPixels();
            Texture.SetPixels(x+padding, y+padding, width-padding*2, height-padding *2, colors);

            IsApplied = false;

            return true;
        }

        #endregion

        #region Read

        public Sprite GetSprite(string name,Vector2 pilot)
        {
            if (IsApplied == false)
                Apply();

            int index = names.FindIndex(findName => findName == name);

            if (index == -1)
                return null;

            Rect rect = rectsPack.usedRectangles[index];
            rect.x += padding;
            rect.y += padding;
            rect.width -= padding * 2;
            rect.height -= padding * 2;

            Sprite sprite = UnityEngine.Sprite.Create(Texture, rect, pilot);
            sprite.name = name;

            return sprite;
        }

        public Sprite GetSprite(string name)
        {
            return GetSprite(name, new Vector2(0.5f, 0.5f));
        }
        #endregion

        public bool Contains(string name)
        {
            return names.Contains(name);
        }

        public bool Remove(string name)
        {
            int index = names.FindIndex(findName => findName == name);

            if (index == -1)
                return false ;

            names.RemoveAt(index);
            Rect rect = rectsPack.usedRectangles[index];
            rectsPack.usedRectangles.RemoveAt(index);
            rectsPack.freeRectangles.Add(rect);

//#if DYNAMIC_ATLAS_DEBUG
            Color[] colors = new Color[(int)(rect.width * rect.height)];
            for(int i =0;i<colors.Length;i++)
            {
                colors[i] = new Color(1.0f, 0f, 0f, 1.0f);
            }
            Texture.SetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, colors);

            IsApplied = false;
//#endif
            return true;
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(Texture);
            names.Clear();

        }

#region File

        /// <summary>
        /// Save to disk the DynamicAtlas.
        /// </summary>
        /// <param name="atlas">DynamicAtlas.</param>
        /// <param name="info">Custom FileInfo.</param>
        /// <returns>Return FileInfo (Path and Name of saved atlas).</returns>
        public static FileInfo Save(DynamicAtlas atlas, FileInfo info = null)
        {
            if (info == null)
                info = new FileInfo(atlas.Texture.name);

            if (Directory.Exists(info.Path) == false)
                Directory.CreateDirectory(info.Path);

            if (atlas.IsApplied == false)
                atlas.Apply();

            byte[] bytes = atlas.Texture.EncodeToPNG();
            string json = JsonUtility.ToJson(atlas);

            File.WriteAllBytes(info.PathTexture, bytes);
            File.WriteAllText(info.PathData, json);

            return info;
        }

        /// <summary>
        /// Load from disk the DynamicAtlas.
        /// </summary>
        /// <param name="info">FileInfo.</param>
        /// <returns>Return DynamicAtlas.</returns>
        public static DynamicAtlas Load(FileInfo info)
        {
            if (File.Exists(info.PathTexture) == false || File.Exists(info.PathData) == false)
                return null;

            byte[] bytes = File.ReadAllBytes(info.PathTexture);
            string json = File.ReadAllText(info.PathData);

            DynamicAtlas atlas = JsonUtility.FromJson<DynamicAtlas>(json);
            atlas.Texture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            atlas.Texture.LoadImage(bytes);
            atlas.Texture.name = info.Name;

            return atlas;
        }

        /// <summary>
        /// Delete the DynamicAtlas on disk by FileInfo.
        /// </summary>
        /// <param name="info">FileInfo.</param>
        /// <returns>Return true if has been deleted else false.</returns>
        public static bool Delete(FileInfo info)
        {
            if (File.Exists(info.PathTexture) == false && File.Exists(info.PathData) == false)
                return false;

            File.Delete(info.PathTexture);
            File.Delete(info.PathData);

            return true;
        }

 

        [System.Serializable]
        public class FileInfo
        {

            static string defaultPath = Application.persistentDataPath + "/DynamicAtlases/";
            static string extensionTexture = ".png", extentionData = ".json";

            [SerializeField]
            string name;
            [SerializeField]
            string path;

            public string Name { get { return name; } }

            public string Path { get { return path; } }

            public string PathTexture { get { return Path + Name + extensionTexture; } }

            public string PathData { get { return Path + Name + extentionData; } }

            public FileInfo(string name, string path = null)
            {
                this.name = name;
                this.path = (string.IsNullOrEmpty(path)) ? defaultPath : path;
            }
        }
#endregion

    }
}
