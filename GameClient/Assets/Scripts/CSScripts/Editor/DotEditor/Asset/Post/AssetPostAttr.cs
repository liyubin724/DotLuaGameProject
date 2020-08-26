using System;

namespace DotEditor.Asset.Post
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class AssetPostRulerMenuAttribute:Attribute
    {
        public string MenuName { get; set; }
        public string FileName { get; set; }

        public AssetPostRulerMenuAttribute(string menuName,string fileName)
        {
            MenuName = menuName;
            FileName = fileName;
        }
    }
}
