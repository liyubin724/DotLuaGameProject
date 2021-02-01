using System;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    public enum FavouriteFolderAssetName
    {
        Prefabs = 0,
        Scenes = 1,
        Scripts = 2,
        Extensions = 3,
        Plugins = 4,
        Textures = 5,
        Materials = 6,
        Audio = 7,
        Brackets = 8,
        Fonts = 9,
        Editor = 10,
        Resources = 11,
        Shaders = 12,
        Terrains = 13,
        Meshes = 14,
    }

    [Serializable]
    public class FavouriteFolderAsset
    {
        public FavouriteFolderAssetName Name = FavouriteFolderAssetName.Prefabs;
        public Texture2D SmallIcon;
        public Texture2D LargeIcon;
    }
}
