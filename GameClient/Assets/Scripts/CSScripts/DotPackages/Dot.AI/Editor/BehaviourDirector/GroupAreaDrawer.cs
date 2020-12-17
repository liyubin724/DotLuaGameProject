using DotEditor.GUIExtension;
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
    public class GroupAreaDrawer : BDDrawer
    {
        private const float HEADER_HEIGHT = 22.0f;

        public override void OnGUI(Rect rect)
        {
            Rect headerRect = new Rect(rect.x, rect.y, rect.width, HEADER_HEIGHT);
            EGUI.DrawBoxHeader(headerRect, Contents.headerContent);
        }

        class Contents
        {
            public static string headerContent = "Track Groups";
        }
    }
}
