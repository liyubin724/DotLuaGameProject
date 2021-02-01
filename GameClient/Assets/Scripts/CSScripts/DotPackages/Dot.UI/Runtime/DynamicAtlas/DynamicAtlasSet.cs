using DotEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEngine.UI.DynAtlas
{
    public class SpriteData
    {
        private string m_SpriteName;
        private string m_AtlasName;
        private WeakReference<Sprite> m_SpriteWeakRef = null;

        public SpriteData(string spriteName,string atlasName,Sprite sprite)
        {
            m_SpriteName = spriteName;
            m_AtlasName = atlasName;
            SetSprite(sprite);
        }

        public string AtlasName { get => m_AtlasName; }

        public void SetSprite(Sprite sprite)
        {
            if(m_SpriteWeakRef == null)
            {
                m_SpriteWeakRef = new WeakReference<Sprite>(sprite, false);
            }else
            {
                m_SpriteWeakRef.SetTarget(sprite);
            }
        }

        public Sprite GetSprite()
        {
            if(m_SpriteWeakRef !=null && m_SpriteWeakRef.TryGetTarget(out Sprite sprite))
            {
                if(sprite.IsNull())
                {
                    return null;
                }else
                {
                    return sprite;
                }
            }
            return null;
        }

        public bool IsValid()
        {
            if(m_SpriteWeakRef == null)
            {
                return false;
            }
            if(m_SpriteWeakRef.TryGetTarget(out Sprite sprite))
            {
                return !sprite.IsNull();
            }
            return false;
        }
    }

    public class DynamicAtlasSet
    {
        private string name;
        private int width;
        private int height;
        private int padding;
        private TextureFormat textureFormat;

        private int maxIndex = 0;

        private Dictionary<string, DynamicAtlas> atlasDic = new Dictionary<string, DynamicAtlas>();
        private Dictionary<string, SpriteData> spriteDic = new Dictionary<string, SpriteData>();

        public DynamicAtlasSet(string name,int size):this(name,size,size,2,TextureFormat.ARGB32)
        {
        }

        public DynamicAtlasSet(string name,int width,int height,int padding, TextureFormat format)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.padding = padding;
            this.textureFormat = format;
        }

        public Sprite AddTexture(Texture2D texture,string name = null)
        {
            if(texture == null)
            {
                DebugLog.Error("DynamicAtlas", "texture is null");
                return null;
            }
            if(!texture.isReadable)
            {
                DebugLog.Error("DynamicAtlas", "texture is not readable.");
                return null;
            }
            name = name ?? texture.name;
            if(spriteDic.TryGetValue(name,out SpriteData spriteData))
            {
                Sprite sprite = spriteData.GetSprite();
                if(sprite == null)
                {
                    DynamicAtlas atlas = atlasDic[spriteData.AtlasName];
                    sprite = atlas.GetSprite(name);
                    spriteData.SetSprite(sprite);
                }
                return sprite;
            }else
            {
                bool isAdded = false;
                
                foreach(var kvp in atlasDic)
                {
                    if(kvp.Value.Write(texture,name))
                    {
                        spriteData = new SpriteData(name, kvp.Key, kvp.Value.GetSprite(name));
                        isAdded = true;
                        break;
                    }
                }

                if(!isAdded)
                {
                    DynamicAtlas atlas = CreateAtlas();
                    atlasDic.Add(atlas.Name, atlas);

                    if(atlas.Write(texture,name))
                    {
                        spriteData = new SpriteData(name, atlas.Name,atlas.GetSprite(name));
                    }
                }

                if(spriteData == null)
                {
                    DebugLog.Error("DynamicAtlas", "spriteData is null.name = "+name);
                    return null;
                }else
                {
                    spriteDic.Add(name, spriteData);
                }
                return spriteData.GetSprite();
            }
        }

        public void UnloadUnused()
        {
            string[] spriteKeys = spriteDic.Keys.ToArray();
            if(spriteKeys!=null && spriteKeys.Length>0)
            {
                foreach(var key in spriteKeys)
                {
                    SpriteData spriteData = spriteDic[key];
                    if(!spriteData.IsValid())
                    {
                        atlasDic[spriteData.AtlasName].Remove(key);
                        spriteDic.Remove(key);
                    }
                }

                string[] atlasKeys = atlasDic.Keys.ToArray();
                foreach(var key in atlasKeys)
                {
                    DynamicAtlas atlas = atlasDic[key];
                    if(atlas.Lenght == 0)
                    {
                        atlasDic.Remove(key);
                        atlas.Dispose();
                    }
                }

            }
        }

        private DynamicAtlas CreateAtlas()
        {
            string atlasName = $"{name}_{maxIndex}";
            ++maxIndex;
            return new DynamicAtlas(atlasName, width, height, padding, textureFormat); ;
        }

        public void WriteTo(string dir)
        {
            foreach(var kvp in atlasDic)
            {
                DynamicAtlas.Save(kvp.Value, new DynamicAtlas.FileInfo(kvp.Value.Name, dir));
            }
        }
    }
}
