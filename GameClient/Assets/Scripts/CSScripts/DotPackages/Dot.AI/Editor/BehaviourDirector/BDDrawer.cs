using DotEngine.BD.Datas;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public abstract class BDDrawer
    {
        public EditorWindow Window { get; set; }
        public CutsceneEditorData EditorData { get; set; }
        public BDDrawer ParentDrawer { get; set; }
        public BDData Data { get; set; }

        public T GetData<T>() where T: BDData
        {
            return (T)Data;
        }

        public abstract void OnGUI(Rect rect);
    }
}
