using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    [CreateAssetMenu(fileName ="folder_config",menuName ="EGUI/Colorfull Folders/Create Folder Config")]
    public class FavouriteFolderConfig : ScriptableObject
    {
        public List<FavouriteFolderColor> colorFolders = new List<FavouriteFolderColor>();
        public List<FavouriteFolderPlatform> platformFolders = new List<FavouriteFolderPlatform>();
        public List<FavouriteFolderTag> tagFolders = new List<FavouriteFolderTag>();
        public List<FavouriteFolderAsset> assetFolders = new List<FavouriteFolderAsset>();
    }
}
