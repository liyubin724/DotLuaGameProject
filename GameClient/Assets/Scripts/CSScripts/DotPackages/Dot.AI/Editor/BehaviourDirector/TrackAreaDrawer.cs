//using DotEditor.GUIExtension;
//using DotEngine.AI.BD.Tracks;
//using UnityEditor;
//using UnityEngine;

//namespace DotEditor.AI.BD
//{
//    public class TrackAreaDrawer
//    { 
//        private const int TRACK_TITLE_HEIGHT = 22;

//        private EditorWindow m_Window = null;

//        public TrackAreaDrawer(EditorWindow win)
//        {
//            m_Window = win;
//        }

//        internal void OnGUI(Rect rect)
//        {
//            EGUI.DrawAreaLine(rect, Color.gray);

//            Rect titleRect = new Rect(rect.x, rect.y, rect.width, TRACK_TITLE_HEIGHT);
//            EditorGUI.LabelField(titleRect, Contents.titleContent, Styles.toolbar);

//            EditorData data = EditorData.Data;
//            DirectorSetting setting = DirectorSetting.GetSetting();

//            Rect clipRect = new Rect(rect.x, titleRect.y + titleRect.height, rect.width, rect.height - TRACK_TITLE_HEIGHT);
//            if (data.CurrentDirector == null)
//            {
//                return;
//            }

//            int selectedIndex = -1;
//            using(new GUI.ClipScope(clipRect))
//            {
//                int trackStartIndex = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
//                int trackEndIndex = Mathf.CeilToInt((setting.ScrollPosY + clipRect.height) / setting.TracklineHeight);
//                for(int i = trackStartIndex;i<trackEndIndex;++i)
//                {
//                    float y = setting.TracklineHeight * i - setting.ScrollPosY;
//                    if(i>=data.CurrentDirector.Groups.Count)
//                    {
//                        break;
//                    }

//                    Track track = data.CurrentDirector.Groups[i];

//                    Rect trackIndexRect = new Rect(0, y, rect.width, setting.TracklineHeight);
//                    GUI.Label(trackIndexRect, $"{(track.Name ?? "")} ({i.ToString()})", data.SelectedTrackIndex == i ? "flow node 1" : "flow node 0");

//                    if(Event.current.type == EventType.MouseUp && trackIndexRect.Contains(Event.current.mousePosition))
//                    {
//                        selectedIndex = i;
//                    }
//                }
//            }

//            if(clipRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseUp)
//            {
//                if(selectedIndex >= 0 && Event.current.button == 0)
//                {
//                    EditorData.Data.SelectedTrackIndex = selectedIndex;
//                }else if(Event.current.button == 1)
//                {
//                    GenericMenu menu = new GenericMenu();
//                    menu.AddItem(new GUIContent(selectedIndex >= 0 ? "Insert" : "New"), false, () =>
//                    {
                        
//                    });
//                    if (selectedIndex>=0)
//                    {
//                        menu.AddItem(new GUIContent("Delete"), false, () =>
//                        {
                            
//                        });

//                        if(selectedIndex>0)
//                        {
//                            menu.AddItem(new GUIContent("Up"), false, () =>
//                            {

//                            });
//                        }
//                        if(selectedIndex < EditorData.Data.CurrentDirector.Groups.Count-1)
//                        {
//                            menu.AddItem(new GUIContent("Down"), false, () =>
//                            {

//                            });
//                        }
//                    }
//                    menu.ShowAsContext();
//                }

//                Event.current.Use();
//                m_Window.Repaint();
//            }
//        }

//        class Contents
//        {
//            public static GUIContent titleContent = new GUIContent("Tracks");
//        }

//        class Styles
//        {
//            public static GUIStyle toolbar;

//            static Styles()
//            {
//                toolbar = new GUIStyle(EditorStyles.toolbar);
//                toolbar.alignment = TextAnchor.MiddleCenter;
//            }
//        }
//    }
//}
