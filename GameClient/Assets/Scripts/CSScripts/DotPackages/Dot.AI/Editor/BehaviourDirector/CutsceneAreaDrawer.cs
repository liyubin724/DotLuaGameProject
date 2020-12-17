using DotEditor.GUIExtension;
using DotEngine.BD.Datas;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public class CutsceneAreaDrawer : BDDrawer
    {
        private const float MIN_PROPERTY_WIDTH = 200;
        private const float MAX_PROPERTY_WIDTH = 400;

        private const float MIN_GROUP_WIDTH = 100;
        private const float MAX_GROUP_WIDTH = 200;

        private const float DRAG_LINE_WIDTH = 3;
        private const float HEADER_HEIGHT = 20;
        private const float GROUP_HEADER_HEIGHT = 20;
        private const float GROUP_PADDING_HEIGHT = 4;

        private Rect m_GroupDragLineRect = new Rect();
        private Rect m_PropertyDragLineRect = new Rect();

        private EGUIDragLine m_GroupDragLine = null;
        private EGUIDragLine m_PropertyDragLine = null;

        public CutsceneAreaDrawer(EditorWindow win,CutsceneData data) 
        {
        }

        private void DrawGroupDragLine(Rect rect)
        {
            if (m_GroupDragLine == null)
            {
                float dragRectX = rect.x + MIN_GROUP_WIDTH;
                float dragRectY = rect.y + 3;
                float dragRectWidth = DRAG_LINE_WIDTH;
                float dragRectHeight = rect.height - 6;
                m_GroupDragLineRect = new Rect(dragRectX, dragRectY, dragRectWidth, dragRectHeight);

                float minDragRectX = rect.x + MIN_GROUP_WIDTH;
                float maxDragRectX = rect.x + MAX_GROUP_WIDTH;

                m_GroupDragLine = new EGUIDragLine(Window, EGUIDirection.Vertical)
                {
                    MinValue = minDragRectX,
                    MaxValue = maxDragRectX,
                };
            }
            m_GroupDragLineRect = m_GroupDragLine.OnGUI(m_GroupDragLineRect);
        }

        private void DrawPropertyDragLine(Rect rect)
        {
            if (m_PropertyDragLine == null)
            {
                float dragRectX = rect.width - MIN_PROPERTY_WIDTH - DRAG_LINE_WIDTH;
                float dragRectY = rect.y + 3;
                float dragRectWidth = DRAG_LINE_WIDTH;
                float dragRectHeight = rect.height - 6;
                m_PropertyDragLineRect = new Rect(dragRectX, dragRectY, dragRectWidth, dragRectHeight);

                float minDragRectX = rect.width - MAX_PROPERTY_WIDTH - DRAG_LINE_WIDTH;
                float maxDragRectX = rect.width - MIN_PROPERTY_WIDTH - DRAG_LINE_WIDTH;
                m_PropertyDragLine = new EGUIDragLine(Window, EGUIDirection.Vertical)
                {
                    MinValue = minDragRectX,
                    MaxValue = maxDragRectX,
                };
            }
            m_PropertyDragLineRect = m_PropertyDragLine.OnGUI(m_PropertyDragLineRect);
        }

        private void DrawGroups(Rect rect)
        {
            float groupHeaderRectX = rect.x;
            float groupHeaderRectY = rect.y;
            float groupHeaderRectWidth = rect.width;
            float groupHeaderRectHeight = HEADER_HEIGHT;
            EGUI.DrawBoxHeader(new Rect(groupHeaderRectX, groupHeaderRectY, groupHeaderRectWidth, groupHeaderRectHeight), Contents.groupHeaderContent);

            CutsceneData cutscene = GetData<CutsceneData>();
            Rect addBtnRect = new Rect(rect.x + rect.width - HEADER_HEIGHT, rect.y, HEADER_HEIGHT, HEADER_HEIGHT);
            if(GUI.Button(addBtnRect,"+",EditorStyles.popup))
            {
                MenuUtility.ShowCreateTrackGroupMenu((groupData) =>
                {
                    cutscene.Groups.Add(groupData);
                });
            }

            Rect clipRect = new Rect(rect.x, rect.y + HEADER_HEIGHT, rect.width, rect.height - HEADER_HEIGHT);
            EGUI.DrawAreaLine(clipRect, Color.grey);

            DrawerSetting setting = DrawerSetting.GetSetting();
            using(var clipScope = new GUI.ClipScope(clipRect))
            {
                float groupRectX = 0.0f;
                float groupRectY = setting.ScrollPosY;
                float groupRectWidth = clipRect.width;
                float groupRectHeight = 0.0f;
                for(int i =0;i<cutscene.Groups.Count;++i)
                {
                    GroupData groupData = cutscene.Groups[i];

                    groupRectY += groupRectHeight + GROUP_PADDING_HEIGHT;
                    groupRectHeight = GROUP_HEADER_HEIGHT + groupData.Tracks.Count * setting.TracklineHeight;

                    DrawGroup(new Rect(groupRectX, groupRectY, groupRectWidth, groupRectHeight), i,groupData);
                }
            }
        }

        private void DrawGroup(Rect rect,int index,GroupData groupData)
        {
            Rect groupHeaderRect = new Rect(rect.x, rect.y, rect.width, GROUP_HEADER_HEIGHT);
            EGUI.DrawBoxHeader(groupHeaderRect, $"{groupData.Name}({index})");

            Rect addBtnRect = new Rect(rect.x + rect.width - GROUP_HEADER_HEIGHT, rect.y, GROUP_HEADER_HEIGHT, GROUP_HEADER_HEIGHT);
            if (GUI.Button(addBtnRect, "+", EditorStyles.popup))
            {
                MenuUtility.ShowCreateTrackMenu(groupData.GetType(),(trackData) =>
                {
                    groupData.Tracks.Add(trackData);
                });
            }
        }

        private void DrawTrack(Rect rect,TrackData trackData)
        {

        }

        private void DrawProperties(Rect rect)
        {
            float propertyHeaderRectX = rect.x;
            float propertyHeaderRectY = rect.y;
            float propertyHeaderRectWidth = rect.width;
            float propertyHeaderRectHeight = HEADER_HEIGHT;
            EGUI.DrawBoxHeader(new Rect(propertyHeaderRectX, propertyHeaderRectY, propertyHeaderRectWidth, propertyHeaderRectHeight), Contents.propertyHeaderContent);
        }

        public override void OnGUI(Rect rect)
        {
            DrawGroupDragLine(rect);
            DrawPropertyDragLine(rect);

            float groupRectX = rect.x;
            float groupRectY = rect.y;
            float groupRectWidth = m_GroupDragLineRect.x;
            float groupRectHeight = rect.height;
            DrawGroups(new Rect(groupRectX, groupRectY, groupRectWidth, groupRectHeight));

            float propertyRectX = m_PropertyDragLineRect.x + m_PropertyDragLineRect.width;
            float propertyRectY = rect.y;
            float propertyRectWidth = rect.width - propertyRectX;
            float propertyRectHeight = rect.height;
            Rect propertyRect = new Rect(propertyRectX, propertyRectY, propertyRectWidth, propertyRectHeight);
            DrawProperties(propertyRect);

        }
        
        class Contents
        {
            public static string groupHeaderContent = "Groups";
            public static string propertyHeaderContent = "Properties";
        }
    }
}
