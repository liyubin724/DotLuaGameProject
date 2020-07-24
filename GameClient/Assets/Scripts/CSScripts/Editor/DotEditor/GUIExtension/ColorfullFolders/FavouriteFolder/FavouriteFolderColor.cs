using System;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    public enum FavouriteFolderColorName
    {
        Red = 0,
        Vermilion = 1,
        Orange = 2,
        YellowOrange = 3,
        Yellow = 4,
        Lime = 5,
        Green = 6,
        BondiBlue = 7,
        Blue = 8,
        Indigo = 9,
        Violet = 10,
        Magenta = 11,
    }

    [Serializable]
    public class FavouriteFolderColor
    {
        public FavouriteFolderColorName Name = FavouriteFolderColorName.Red;
        public Texture2D SmallIcon;
        public Texture2D LargeIcon;
    }
}
