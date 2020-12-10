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
        private ActionDragType m_DragType = ActionDragType.None;

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
