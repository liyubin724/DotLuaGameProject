//using DotEditor.GUIExtension;
//using DotEngine.AI.BD;
//using DotEngine.AI.BD.Tracks;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//namespace DotEditor.AI.BD
//{
//    public class EditorData
//    {
//        public static EditorData Data = null;

//        public event Action OnDirectorChanged;
//        private Cutscene m_CurrentDirector = null;
//        public Cutscene CurrentDirector
//        {
//            get
//            {
//                return m_CurrentDirector;
//            }
//            set
//            {
//                m_CurrentDirector = value;
//                OnDirectorChanged?.Invoke();
//            }
//        }

//        public event Action OnSelectedTrackIndexChanged;
//        private int m_SelectedTrackIndex = -1;
//        public int SelectedTrackIndex 
//        {
//            get
//            {
//                return m_SelectedTrackIndex;
//            }
//            set
//            {
//                m_SelectedTrackIndex = value;
//                OnSelectedTrackIndexChanged?.Invoke();
//            }
//        }

//        public Track GetSelectedTrack()
//        {
//            if(CurrentDirector!=null && SelectedTrackIndex>=0)
//            {
//                return CurrentDirector.Groups[SelectedTrackIndex];
//            }

//            return null;
//        }
//    }

//    public class DirectorWindow : EditorWindow
//    {
//        private const int TOOLBAR_HEIGHT = 20;

//        private const float MIN_TRACK_WIDTH = 200;
//        private const float MAX_TRACK_WIDTH = 300;

//        private const float MIN_PROPERTY_HEIGHT = 100;
//        private const float MAX_PROPERTY_HEIGHT = 300;

//        private const float DRAG_LINE_SIZE= 3;

//        [MenuItem("Game/AI/Behaviour Director")]
//        static void ShowWin()
//        {
//            var win = GetWindow<DirectorWindow>();
//            win.titleContent = new GUIContent("Behaviour Director");
//            win.Show();
//        }

//        private Rect m_TrackDragRect = new Rect();
//        private Rect m_PropertyDragRect = new Rect();

//        private EGUIDragLine m_TrackDragLine = null;
//        private EGUIDragLine m_PropertyDragLine = null;

//        private Rect m_ToolbarRect = new Rect();
//        private Rect m_TrackRect = new Rect();
//        private Rect m_TrackPropertyRect = new Rect();
//        private Rect m_ActionRect = new Rect();
//        private Rect m_ActionPropertyRect = new Rect();

//        private TrackAreaDrawer m_TrackAreaDrawer = null;
//        private TrackPropertyAreaDrawer m_TrackPropertyAreaDrawer = null;


//        private void OnEnable()
//        {
//            EditorData.Data = new EditorData();

//            m_TrackAreaDrawer = new TrackAreaDrawer(this);
//            m_TrackPropertyAreaDrawer = new TrackPropertyAreaDrawer(this);

//            Cutscene director = new Cutscene();
//            EditorData.Data.CurrentDirector = director;

//            Track track = new Track();
//            track.Category = NodeTrackCategory.Actor;
//            track.Name = "Actor Group";
//            director.Groups.Add(track) ;

//            track = new Track();
//            director.Groups.Add(track);

//            track = new Track();
//            director.Groups.Add(track);
//        }

//        private void OnGUI()
//        {
//            m_ToolbarRect = new Rect(0, 0, position.width, TOOLBAR_HEIGHT);
//            DrawToolbar(m_ToolbarRect);

//            if (m_TrackDragLine == null)
//            {
//                m_TrackDragRect = new Rect(MIN_TRACK_WIDTH, m_ToolbarRect.y + m_ToolbarRect.height, DRAG_LINE_SIZE, position.height - m_ToolbarRect.y - m_ToolbarRect.height);
//                m_TrackDragLine = new EGUIDragLine(this, EGUIDirection.Vertical);
//            }

//            if (m_PropertyDragLine == null)
//            {
//                m_PropertyDragRect = new Rect(0, position.height - MIN_PROPERTY_HEIGHT - DRAG_LINE_SIZE, position.width, DRAG_LINE_SIZE);
//                m_PropertyDragLine = new EGUIDragLine(this, EGUIDirection.Horizontal);
//            }

//            m_TrackDragRect = m_TrackDragLine.OnGUI(m_TrackDragRect);
//            if (m_TrackDragRect.x > MAX_TRACK_WIDTH)
//            {
//                m_TrackDragRect.x = MAX_TRACK_WIDTH;
//            } else if (m_TrackDragRect.x < MIN_TRACK_WIDTH)
//            {
//                m_TrackDragRect.x = MIN_TRACK_WIDTH;
//            }
//            m_PropertyDragRect = m_PropertyDragLine.OnGUI(m_PropertyDragRect);
//            if (m_PropertyDragRect.y < position.height - MAX_PROPERTY_HEIGHT - DRAG_LINE_SIZE)
//            {
//                m_PropertyDragRect.y = position.height - MAX_PROPERTY_HEIGHT - DRAG_LINE_SIZE;
//            } else if (m_PropertyDragRect.y > position.height - MIN_PROPERTY_HEIGHT - DRAG_LINE_SIZE)
//            {
//                m_PropertyDragRect.y = position.height - MIN_PROPERTY_HEIGHT - DRAG_LINE_SIZE;
//            }

//            float trackRectX = 0.0f;
//            float trackRectY = m_ToolbarRect.y + m_ToolbarRect.height;
//            float trackRectWidth = m_TrackDragRect.x;
//            float trackRectHeight = m_PropertyDragRect.y - TOOLBAR_HEIGHT;
//            m_TrackRect = new Rect(trackRectX,trackRectY,trackRectWidth,trackRectHeight);
//            m_TrackAreaDrawer.OnGUI(m_TrackRect);

//            float trackPropertyRectX = trackRectX;
//            float trackPropertyRectY = m_PropertyDragRect.y + m_PropertyDragRect.height;
//            float trackPropertyRectWidth = trackRectWidth;
//            float trackPropertyRectHeight = position.y + position.height - m_PropertyDragRect.y - m_PropertyDragRect.height;
//            m_TrackPropertyRect = new Rect(trackPropertyRectX,trackPropertyRectY, trackPropertyRectWidth, trackPropertyRectHeight);
//            m_TrackPropertyAreaDrawer.OnGUI(m_TrackPropertyRect);

//            float actionPropertyRectX = m_TrackDragRect.x + m_TrackDragRect.width;
//            float actionPropertyRectY = trackPropertyRectY;
//            float actionPropertyRectWidth = position.width - m_TrackDragRect.x - m_TrackDragRect.width;
//            float actionPropertyRectHeight = trackPropertyRectHeight;
//            m_ActionPropertyRect = new Rect(actionPropertyRectX, actionPropertyRectY, actionPropertyRectWidth, actionPropertyRectHeight);
//            EGUI.DrawAreaLine(m_ActionPropertyRect, Color.blue);

//            float actionRectX = actionPropertyRectX;
//            float actionRectY = trackRectY;
//            float actionRectWidth = actionPropertyRectWidth;
//            float actionRectHeight = trackRectHeight;
//            m_ActionRect = new Rect(actionRectX, actionRectY, actionRectWidth, actionRectHeight);
//            EGUI.DrawAreaLine(m_ActionRect, Color.blue);
//        }

//        private void DrawToolbar(Rect rect)
//        {
//            EditorGUI.LabelField(rect, GUIContent.none, EditorStyles.toolbar);
//            EditorGUI.LabelField(rect, Contents.titleContent, Styles.titleStyle);

//            GUILayout.BeginArea(rect);
//            {
//                EditorGUILayout.BeginHorizontal();
//                {
//                    if (GUILayout.Button(Contents.createContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    if (GUILayout.Button(Contents.openContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    if (GUILayout.Button(Contents.saveContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    if (GUILayout.Button(Contents.saveToContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    GUILayout.FlexibleSpace();

//                    if (GUILayout.Button(Contents.zoomInContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    if (GUILayout.Button(Contents.zoomOutContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    if (GUILayout.Button(Contents.settingContent, EditorStyles.toolbarButton))
//                    {

//                    }

//                    if (GUILayout.Button(Contents.helpContent, EditorStyles.toolbarButton))
//                    {

//                    }
//                }
//                EditorGUILayout.EndHorizontal();
//            }
//            GUILayout.EndArea();
//        }

//    }

//    class Contents
//    {
//        public static GUIContent titleContent = new GUIContent("Director");

//        public static GUIContent createContent = new GUIContent("Create", "Create New");
//        public static GUIContent openContent = new GUIContent("Open", "Open");
//        public static GUIContent saveContent = new GUIContent("Save", "Save");
//        public static GUIContent saveToContent = new GUIContent("Save To", "Save To");

//        public static GUIContent helpContent = new GUIContent("?", "Show help");
//        public static GUIContent settingContent = new GUIContent("Setting", "Open Setting Window");
//        public static GUIContent zoomInContent = new GUIContent("+", "Zoom in");
//        public static GUIContent zoomOutContent = new GUIContent("-", "Zoom out");

//    }

//    class Styles
//    {
//        public static GUIStyle titleStyle = null;
//        public static GUIStyle propertyStyle = null;

//        static Styles()
//        {
//            titleStyle = new GUIStyle(EditorStyles.label);
//            titleStyle.alignment = TextAnchor.MiddleCenter;
//        }
//    }
//}
