using DotEngine.BD.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public class ActionAreaDrawer : AreaDrawer
    {
        public ActionData Data { get; set; }
        private ActionDragType m_DragType = ActionDragType.None;

        public ActionAreaDrawer(EditorWindow win,ActionData data) : base(win)
        {
            Data = data;
        }

        public override void OnGUI(Rect rect)
        {
            
        }

        enum ActionDragType
        {
            None = 0,
            Item,
            ItemLeft,
            ItemRight,
        }
    }
}
