using System;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    public enum FavouriteFolderTagName
    {
        Red = 0,
        Vermilion = 1,
        Orange = 2,
        YellowOrange = 3,
        Yellow = 4,
        Lime = 5,
        Green = 6,
        Cyan = 7,
        Blue = 8,
        DarkBlue = 9,
        Violet = 10,
        Magenta = 11,
    }

    [Serializable]
    public class FavouriteFolderTag
    {
        public FavouriteFolderTagName Name = FavouriteFolderTagName.Red;
        public Texture2D SmallIcon;
        public Texture2D LargeIcon;
    }
}
