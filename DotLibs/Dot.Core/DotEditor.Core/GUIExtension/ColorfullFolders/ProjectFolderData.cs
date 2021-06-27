using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    public enum FolderType
    {
        Name,
        Path,
    }

    [Serializable]
    public class ProjectFolderData
    {
        public FolderType Type;
        public string Value;

        public Texture2D SmallIcon;
        public Texture2D LargeIcon;
    }
}
