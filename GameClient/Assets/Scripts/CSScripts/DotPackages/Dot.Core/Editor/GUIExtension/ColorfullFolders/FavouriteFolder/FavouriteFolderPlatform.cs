using System;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    public enum FavouriteFolderPlatformName
    {
        Android = 0,
        iOS = 1,
        Mac = 2,
        WebGL = 3,
        Windows = 4
    }

    [Serializable]
    public class FavouriteFolderPlatform
    {
        public FavouriteFolderPlatformName Name = FavouriteFolderPlatformName.Windows;
        public Texture2D SmallIcon;
        public Texture2D LargeIcon;
    }
}
