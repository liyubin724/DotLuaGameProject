using DotEngine.BD.Datas;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public abstract class AreaDrawer
    {
        public EditorWindow Window { get; set; }
        public AreaDrawer ParentDrawer { get; set; }
        public BDData Data { get; set; }

        public abstract void OnGUI(Rect rect);
    }
}
