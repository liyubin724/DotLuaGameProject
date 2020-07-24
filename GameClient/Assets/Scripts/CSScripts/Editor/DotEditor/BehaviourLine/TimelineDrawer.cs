using DotEditor.GUIExtension;
using DotEngine.BehaviourLine.Line;
using DotEngine.BehaviourLine.Track;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class TimelineDrawer
    {
        private const int TOOLBAR_HEIGHT = 20;
        private const int TRACK_TITLE_HEIGHT = 20;
        private const int PROPERTY_TITLE_HEIGHT = 20;
        private const int LINE_RULER_HEIGHT = 20;

        private const float MIN_TRACK_WIDTH = 100;
        private const float MAX_TRACK_WIDTH = 300;

        private const float MIN_PROPERTY_WIDTH = 200;
        private const float MAX_PROPERTY_WIDTH = 400;

        private const float DRAG_WIDTH = 5;

        public TimelineData Data { get; private set; }

        public EditorWindow Window { get; private set; }

        private LineSetting setting;
        private string titleName = string.Empty;
        private List<TracklineDrawer> trackDrawers = new List<TracklineDrawer>();

        private float trackWidth = MIN_TRACK_WIDTH;
        private float propertyWidth = MIN_PROPERTY_WIDTH;

        private bool isTrackDragging = false;
        private bool isPropertyDragging = false;

        private int selectedTrackIndex = -1;
        private string dataFilePath = null;

        public TimelineDrawer(EditorWindow win, string titleName = null)
        {
            Window = win;
            Window.wantsMouseMove = true;

            this.titleName = titleName ?? "Timeline";
            setting = new LineSetting();
            LineSetting.Setting = setting;
        }

        public void SetData(TimelineData data)
        {
            Data = data;

            int maxActionIndex = 0;
            foreach (var track in data.Tracks)
            {
                foreach (var action in track.Actions)
                {
                    if (action.Index > maxActionIndex)
                    {
                        maxActionIndex = action.Index;
                    }
                }
            }
            setting.MaxActionIndex = maxActionIndex;

            trackDrawers.Clear();
            selectedTrackIndex = -1;

            for (int i = 0; i < data.Tracks.Count; ++i)
            {
                TracklineDrawer tracklineDrawer = new TracklineDrawer(this);
                tracklineDrawer.SetData(data.Tracks[i]);

                trackDrawers.Add(tracklineDrawer);
            }
        }

        public void OnGUI(Rect rect)
        {
            Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, TOOLBAR_HEIGHT);
            DrawToolbar(toolbarRect);

            Rect trackRect = new Rect(rect.x, rect.y + TOOLBAR_HEIGHT, trackWidth, rect.height - TOOLBAR_HEIGHT);
            DrawTrack(trackRect);

            Rect trackDragRect = new Rect(trackRect.x + trackRect.width, trackRect.y, DRAG_WIDTH, trackRect.height);
            DrawTrackDrag(trackDragRect);

            Rect propertyRect = new Rect(rect.x + rect.width - propertyWidth, trackRect.y, propertyWidth, trackRect.height);
            DrawProperty(propertyRect);

            Rect propertyDragRect = new Rect(propertyRect.x - DRAG_WIDTH, trackRect.y, DRAG_WIDTH, trackRect.height);
            DrawPropertyDrag(propertyDragRect);

            Rect lineRect = new Rect(trackDragRect.x + trackDragRect.width, trackRect.y, rect.width - trackRect.width - trackDragRect.width - propertyRect.width - propertyDragRect.width, trackRect.height);
            if (lineRect.width > 0)
            {
                DrawLine(lineRect);
            }

            if (GUI.changed)
            {
                Window.Repaint();
            }
        }

        private void DrawToolbar(Rect toolbarRect)
        {
            EditorGUI.LabelField(toolbarRect, GUIContent.none, EditorStyles.toolbar);
            EditorGUI.LabelField(toolbarRect, titleName, Styles.titleStyle);

            Rect createRect = new Rect(toolbarRect.x, toolbarRect.y, 60, toolbarRect.height);
            if (GUI.Button(createRect, Contents.createContent, EditorStyles.toolbarButton))
            {
                dataFilePath = null;
                SetData(new TimelineData()
                {
                    TimeLength = 1.0f,
                    Tracks = new List<TracklineData>()
                    {
                        new TracklineData()
                        {
                            Name = "Track 0",
                        },
                        new TracklineData()
                        {
                            Name = "Track 1",
                        },
                        new TracklineData()
                        {
                            Name = "Track 2",
                        },
                    },
                }) ;

                Window.Repaint();
            }

            Rect openRect = createRect;
            openRect.x += createRect.width;
            if (GUI.Button(openRect, Contents.openContent, EditorStyles.toolbarButton))
            {
                string dir = string.IsNullOrEmpty(dataFilePath) ? "" : Path.GetDirectoryName(dataFilePath);
                string path = EditorUtility.OpenFilePanel("Open", dir, "*.json");
                if(!string.IsNullOrEmpty(path))
                {
                    dataFilePath = path;
                    SetData(LineUtil.ReadFromJsonFile(dataFilePath));
                    Window.Repaint();
                }
            }

            Rect saveRect = openRect;
            saveRect.x += createRect.width;
            if (GUI.Button(saveRect, Contents.saveContent, EditorStyles.toolbarButton))
            {
                if (string.IsNullOrEmpty(dataFilePath))
                {
                    EditorUtility.DisplayDialog("Warning", "The Config is new ,plz use saveto.", "OK");
                }
                else
                {
                    LineUtil.SaveToJsonFile(Data, dataFilePath);
                }
            }

            Rect saveToRect = saveRect;
            saveToRect.x += saveRect.width;
            if (GUI.Button(saveToRect, Contents.saveToContent, EditorStyles.toolbarButton))
            {
                string dir = string.IsNullOrEmpty(dataFilePath) ? "" : Path.GetDirectoryName(dataFilePath);
                string fileName = string.IsNullOrEmpty(dataFilePath) ? "timelinedata" : Path.GetFileNameWithoutExtension(dataFilePath);
                string filePath = EditorUtility.SaveFilePanel("Save to", dir, fileName, ".json");
                if (!string.IsNullOrEmpty(filePath))
                {
                    dataFilePath = filePath;

                    LineUtil.SaveToJsonFile(Data, dataFilePath);
                }
            }


            Rect helpBtnRect = new Rect(toolbarRect.x + toolbarRect.width - 30, toolbarRect.y, 30, toolbarRect.height);
            if (GUI.Button(helpBtnRect, Contents.helpContent, EditorStyles.toolbarButton))
            {

            }

            Rect settingBtnRect = helpBtnRect;
            settingBtnRect.x -= 30;
            if (GUI.Button(settingBtnRect, Contents.settingContent, EditorStyles.toolbarButton))
            {
                LineSettingWindow.ShowWin();
            }

            Rect zoomOutBtnRect = settingBtnRect;
            zoomOutBtnRect.x -= 33;
            if (GUI.Button(zoomOutBtnRect, Contents.zoomOutContent, EditorStyles.toolbarButton))
            {
                setting.TimeStep -= setting.ZoomTimeStep;

                Window.Repaint();
            }

            Rect zoomInBtnRect = zoomOutBtnRect;
            zoomInBtnRect.x -= 30;
            if (GUI.Button(zoomInBtnRect, Contents.zoomInContent, EditorStyles.toolbarButton))
            {
                setting.TimeStep += setting.ZoomTimeStep;
                Window.Repaint();
            }
        }

        private void DrawTrack(Rect trackRect)
        {
            EGUI.DrawAreaLine(trackRect, Color.gray);

            LineSetting setting = LineSetting.Setting;

            Rect titleRect = new Rect(trackRect.x, trackRect.y, trackRect.width, TRACK_TITLE_HEIGHT);
            EditorGUI.LabelField(titleRect, "Tracks", EditorStyles.toolbar);

            if(Data == null)
            {
                return;
            }

            Rect clipRect = new Rect(trackRect.x, trackRect.y + TRACK_TITLE_HEIGHT, trackRect.width, trackRect.height - TRACK_TITLE_HEIGHT);
            using (new GUI.ClipScope(clipRect))
            {
                int start = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
                int end = Mathf.CeilToInt((setting.ScrollPosY + clipRect.height) / setting.TracklineHeight);

                Dictionary<Rect, int> rectIndexDic = new Dictionary<Rect, int>();
                for (int i = start; i < end; ++i)
                {
                    float y = setting.TracklineHeight * i - setting.ScrollPos.y;

                    if (i >= Data.Tracks.Count)
                    {
                        break;
                    }

                    Rect indexRect = new Rect(0, y, trackWidth, setting.TracklineHeight);

                    rectIndexDic.Add(indexRect, i);

                    GUI.Label(indexRect, $"{(Data.Tracks[i].Name ?? "")} ({i.ToString()})", selectedTrackIndex == i ? "flow node 1" : "flow node 0");
                }

                if (Event.current.type == EventType.MouseUp)
                {
                    int index = -1;
                    foreach (var kvp in rectIndexDic)
                    {
                        if (kvp.Key.Contains(Event.current.mousePosition))
                        {
                            index = kvp.Value;
                            break;
                        }
                    }

                    if (Event.current.button == 1)
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent(index >= 0 ? "Insert New" : "Add New"), false, () =>
                        {
                            OnTrackAdded(new TracklineData(), index);
                        });

                        if (index >= 0)
                        {
                            menu.AddItem(new GUIContent("Delete"), false, () =>
                            {
                                OnTrackDeleted(index);
                            });
                        }

                        menu.ShowAsContext();
                    }

                    if (index >= 0)
                    {
                        OnTrackSelected(trackDrawers[index]);
                    }
                    Window.Repaint();
                    Event.current.Use();
                }
            }
        }

        private void DrawTrackDrag(Rect dragRect)
        {
            EGUI.DrawVerticalLine(dragRect, Color.grey, 2.0f);

            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);

            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && dragRect.Contains(Event.current.mousePosition))
                {
                    isTrackDragging = true;

                    Event.current.Use();
                    Window.Repaint();
                }
                else if (isTrackDragging && Event.current.type == EventType.MouseDrag)
                {
                    trackWidth += Event.current.delta.x;
                    if (trackWidth < MIN_TRACK_WIDTH)
                    {
                        trackWidth = MIN_TRACK_WIDTH;
                    }
                    else if (trackWidth > MAX_TRACK_WIDTH)
                    {
                        trackWidth = MAX_TRACK_WIDTH;
                    }
                    Window.Repaint();
                }
                else if (isTrackDragging && Event.current.type == EventType.MouseUp)
                {
                    isTrackDragging = false;
                    Event.current.Use();
                    Window.Repaint();
                }
            }
        }

        private void DrawLine(Rect lineRect)
        {
            EGUI.DrawAreaLine(lineRect, Color.gray);

            Rect rulerRect = new Rect(lineRect.x, lineRect.y, lineRect.width, LINE_RULER_HEIGHT);
            EditorGUI.LabelField(rulerRect, GUIContent.none, EditorStyles.toolbar);
            DrawLineRuler(rulerRect);

            Rect gridRect = new Rect(lineRect.x, lineRect.y + LINE_RULER_HEIGHT, lineRect.width, lineRect.height - LINE_RULER_HEIGHT);
            DrawLineGrid(gridRect);

            if(Data!=null)
            {
                DrawTrackline(gridRect);

                LineSetting setting = LineSetting.Setting;
                using (new GUILayout.AreaScope(gridRect))
                {
                    using (var scop = new UnityEditor.EditorGUILayout.ScrollViewScope(setting.ScrollPos))
                    {
                        float scrollWith = Mathf.Max(Data.TimeLength * setting.WidthForSecond, gridRect.width);
                        float scrollHeight = Mathf.Max(Data.Tracks.Count * setting.TracklineHeight, gridRect.height);

                        GUILayout.Label("", GUILayout.Width(scrollWith), GUILayout.Height(scrollHeight - 20));

                        setting.ScrollPos = scop.scrollPosition;
                    }
                }
            }
            
        }

        private void DrawLineRuler(Rect rulerRect)
        {
            LineSetting setting = LineSetting.Setting;
            using (new GUI.ClipScope(rulerRect))
            {
                int start = Mathf.FloorToInt(setting.ScrollPosX / setting.WidthForSecond);
                int end = Mathf.CeilToInt((setting.ScrollPosX + rulerRect.width) / setting.WidthForSecond);

                int startCount = Mathf.FloorToInt(start / setting.TimeStep);
                int endCount = Mathf.FloorToInt(end / setting.TimeStep);
                for (int i = startCount; i <= endCount; i++)
                {
                    var x = i * setting.TimeStepWidth - setting.ScrollPosX;

                    if (i % 10 == 0)
                    {
                        Handles.color = new Color(0, 0, 0, 0.8f);
                        Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rulerRect.height * 0.8f, 0));
                        GUI.Label(new Rect(x, 5, 40, 40), (i * setting.TimeStep).ToString("F1"));
                    }
                    else if (i % 5 == 0)
                    {
                        Handles.color = new Color(0, 0, 0, 0.5f);
                        Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rulerRect.height * 0.5f, 0));
                    }
                    else
                    {
                        Handles.color = new Color(0, 0, 0, 0.5f);
                        Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rulerRect.height * 0.3f, 0));
                    }
                }
            }
        }

        private void DrawLineGrid(Rect gridRect)
        {
            LineSetting setting = LineSetting.Setting;
            using (new GUI.ClipScope(new Rect(gridRect.x, gridRect.y, gridRect.width, gridRect.height)))
            {
                int startX = Mathf.FloorToInt(setting.ScrollPosX / setting.WidthForSecond);
                int endX = Mathf.CeilToInt((setting.ScrollPosX + gridRect.width) / setting.WidthForSecond);

                int startXCount = Mathf.FloorToInt(startX / setting.TimeStep);
                int endXCount = Mathf.FloorToInt(endX / setting.TimeStep);
                for (int i = startXCount; i <= endXCount; i++)
                {
                    var x = i * setting.TimeStepWidth - setting.ScrollPosX;

                    Color handlesColor = new Color(0, 0, 0, 0.3f);
                    if (i % 10 == 0)
                    {
                        handlesColor = new Color(0, 0, 0, 1.0f);
                    }
                    else if (i % 5 == 0)
                    {
                        handlesColor = new Color(0, 0, 0, 0.8f);
                    }
                    Handles.color = handlesColor;
                    Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, gridRect.height, 0));
                }

                if(Data!=null)
                {
                    float stopLineX = Data.TimeLength * setting.WidthForSecond - setting.ScrollPosX;
                    Handles.color = Color.red;
                    Handles.DrawLine(new Vector3(stopLineX, 0, 0), new Vector3(stopLineX, gridRect.height, 0));
                }

                int startY = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
                int endY = Mathf.CeilToInt((setting.ScrollPosY + gridRect.height) / setting.TracklineHeight);
                for (int i = startY; i <= endY; i++)
                {
                    float y = setting.TracklineHeight * i - setting.ScrollPosY;
                    Handles.color = new Color(0, 0, 0, 0.9f);
                    Handles.DrawLine(new Vector3(0, y, 0), new Vector3(gridRect.width, y, 0));
                }
            }
        }

        private void DrawTrackline(Rect lineRect)
        {
            LineSetting setting = LineSetting.Setting;
            using (new GUI.ClipScope(lineRect))
            {
                int startY = Mathf.FloorToInt(setting.ScrollPosY / setting.TracklineHeight);
                int endY = Mathf.CeilToInt((setting.ScrollPosY + lineRect.height) / setting.TracklineHeight);

                float maxWidth = Data.TimeLength * setting.WidthForSecond;

                for (int i = startY; i < endY; ++i)
                {
                    float y = setting.TracklineHeight * i - setting.ScrollPosY;

                    if (i >= trackDrawers.Count)
                    {
                        break;
                    }

                    Rect tRect = new Rect(0, y, lineRect.width, setting.TracklineHeight);
                    tRect.width = Mathf.Min(lineRect.width, maxWidth - setting.ScrollPosX);
                    if (selectedTrackIndex == i)
                    {
                        EGUI.DrawAreaLine(tRect, Color.green);
                    }

                    trackDrawers[i].OnDrawGUI(tRect);
                }
            }
        }

        private void DrawPropertyDrag(Rect dragRect)
        {
            EGUI.DrawVerticalLine(dragRect, Color.grey, 2.0f);

            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);

            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && dragRect.Contains(Event.current.mousePosition))
                {
                    isPropertyDragging = true;

                    Event.current.Use();
                    Window.Repaint();
                }
                else if (isPropertyDragging && Event.current.type == EventType.MouseDrag)
                {
                    propertyWidth -= Event.current.delta.x;
                    if (propertyWidth < MIN_PROPERTY_WIDTH)
                    {
                        propertyWidth = MIN_PROPERTY_WIDTH;
                    }
                    else if (propertyWidth > MAX_PROPERTY_WIDTH)
                    {
                        propertyWidth = MAX_PROPERTY_WIDTH;
                    }
                    Window.Repaint();
                }
                else if (isPropertyDragging && Event.current.type == EventType.MouseUp)
                {
                    isPropertyDragging = false;
                    Event.current.Use();
                    Window.Repaint();
                }
            }
        }

        private void DrawProperty(Rect propertyRect)
        {
            EGUI.DrawAreaLine(propertyRect, Color.gray);

            LineSetting setting = LineSetting.Setting;

            Rect titleRect = new Rect(propertyRect.x, propertyRect.y, propertyRect.width, TRACK_TITLE_HEIGHT);
            EditorGUI.LabelField(titleRect, "Property", EditorStyles.toolbar);

            if (Data == null)
            {
                return;
            }

            Rect contentRect = new Rect(propertyRect.x, propertyRect.y + TRACK_TITLE_HEIGHT, propertyRect.width, propertyRect.height - TRACK_TITLE_HEIGHT);
            GUILayout.BeginArea(contentRect);
            {
                Data.TimeLength = EditorGUILayout.FloatField("Time Length", Data.TimeLength);

                EditorGUILayout.Space();

                if (selectedTrackIndex >= 0)
                {
                    trackDrawers[selectedTrackIndex].OnDrawProperty(contentRect);
                }
            }
            GUILayout.EndArea();

        }

        internal void OnTrackSelected(TracklineDrawer tracklineDrawer)
        {
            if (tracklineDrawer == null)
            {
                selectedTrackIndex = -1;
            }
            else
            {
                int newSelectedTrackIndex = trackDrawers.IndexOf(tracklineDrawer);
                if (newSelectedTrackIndex != selectedTrackIndex)
                {
                    if (selectedTrackIndex >= 0 && selectedTrackIndex < trackDrawers.Count)
                    {
                        trackDrawers[selectedTrackIndex].IsSelected = false;
                    }
                    selectedTrackIndex = newSelectedTrackIndex;
                }
            }

            Window.Repaint();
        }

        internal void OnTrackAdded(TracklineData tracklineData, int insertIndex = -1)
        {
            if (insertIndex < 0)
            {
                insertIndex = trackDrawers.Count;
            }

            TracklineDrawer tracklineDrawer = new TracklineDrawer(this);
            tracklineDrawer.SetData(tracklineData);

            Data.Tracks.Insert(insertIndex, tracklineData);
            trackDrawers.Insert(insertIndex, tracklineDrawer);
        }

        internal void OnTrackDeleted(int deleteIndex)
        {
            if (deleteIndex >= 0 && deleteIndex < trackDrawers.Count)
            {
                if (deleteIndex == selectedTrackIndex)
                {
                    OnTrackSelected(null);
                    selectedTrackIndex = -1;
                }
                else if (selectedTrackIndex > deleteIndex)
                {
                    selectedTrackIndex--;
                }
                trackDrawers.RemoveAt(deleteIndex);
                Data.Tracks.RemoveAt(deleteIndex);

                Window.Repaint();
            }
        }

        internal void OnTrackMoveup(int moveIndex)
        {

        }

        internal void OnTrackMovedown(int moveIndex)
        {

        }

        class Contents
        {
            public static GUIContent createContent = new GUIContent("Create", "Create New");
            public static GUIContent openContent = new GUIContent("Open", "Open");
            public static GUIContent saveContent = new GUIContent("Save", "Save");
            public static GUIContent saveToContent = new GUIContent("Save To", "Save To");

            public static GUIContent helpContent = new GUIContent("?", "Show help");
            public static GUIContent settingContent = new GUIContent("S", "Open Setting Window");
            public static GUIContent zoomInContent = new GUIContent("+", "Zoom in");
            public static GUIContent zoomOutContent = new GUIContent("-", "Zoom out");

        }

        class Styles
        {
            public static GUIStyle titleStyle = null;
            public static GUIStyle propertyStyle = null;

            static Styles()
            {
                titleStyle = new GUIStyle(EditorStyles.label);
                titleStyle.alignment = TextAnchor.MiddleCenter;
            }
        }
    }
}