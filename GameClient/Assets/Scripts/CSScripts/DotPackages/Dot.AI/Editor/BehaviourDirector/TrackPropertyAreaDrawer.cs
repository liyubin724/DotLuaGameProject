//using DotEditor.GUIExtension;
//using DotEngine.AI.BD.Tracks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEngine;

//namespace DotEditor.AI.BD
//{
//    public class TrackPropertyAreaDrawer
//    {
//        private const int TITLE_HEIGHT = 22;

//        private EditorWindow m_Window = null;

//        public TrackPropertyAreaDrawer(EditorWindow win)
//        {
//            m_Window = win;
//        }

//        private Vector2 m_ScrollPos = Vector2.zero;
//        internal void OnGUI(Rect rect)
//        {
//            Rect titleRect = new Rect(rect.x, rect.y, rect.width, TITLE_HEIGHT);
//            EGUI.DrawBoxHeader(titleRect, Contents.titleStr, EGUIStyles.BoxedHeaderCenterStyle);

//            Track trackData = EditorData.Data.GetSelectedTrack();
//            if(trackData == null)
//            {
//                return;
//            }

//            Rect scrollRect = new Rect(rect.x, titleRect.y + titleRect.height, rect.width, rect.height - titleRect.height);
//            GUILayout.BeginArea(scrollRect);
//            {
//                EGUI.BeginLabelWidth(100);
//                {
//                    trackData.Name = EditorGUILayout.TextField(Contents.nameContent, trackData.Name);
//                }
//                EGUI.EndLableWidth();
//            }
//            GUILayout.EndArea();
//        }

//        class Contents
//        {
//            public static string titleStr= "Property";

//            public static GUIContent nameContent = new GUIContent("Name");
//        }

//        class Styles
//        {

//        }
//    }
//}
