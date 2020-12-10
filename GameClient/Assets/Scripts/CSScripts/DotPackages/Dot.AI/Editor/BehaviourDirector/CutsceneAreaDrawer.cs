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
    public class CutsceneEditorData
    {
        public static CutsceneEditorData EditorData = new CutsceneEditorData();

        private CutsceneData m_SelectedCutsceneData = null;
        public event Action SelectedCutsceneChangedEvent;
        public CutsceneData Cutscene
        {
            get
            {
                return m_SelectedCutsceneData;
            }
            set
            {
                m_SelectedCutsceneData = value;
                SelectedCutsceneChangedEvent?.Invoke();
                
                SelectedTrackGroupIndex = -1;
                SelectedTrackIndex = -1;
                SelectedActionIndex = -1;
            }
        }

        private int m_SelectedTrackGroupIndex = -1;
        private event Action SelectedTrackGroupChangedEvent;
        public int SelectedTrackGroupIndex
        {
            get
            {
                return m_SelectedTrackGroupIndex;
            }
            set
            {
                
                m_SelectedTrackGroupIndex = value;
            }
        }
        private int m_SelectedTrackIndex = -1;
        public int SelectedTrackIndex
        {
            get
            {
                return m_SelectedTrackIndex;
            }
            set
            {
                m_SelectedTrackIndex = value;
            }
        }

        private int m_SelectedActionIndex = -1;
        public int SelectedActionIndex
        {
            get
            {
                return m_SelectedActionIndex;
            }
            set
            {
                m_SelectedActionIndex = value;
            }
        }
    }

    public class CutsceneAreaDrawer : AreaDrawer
    {
        private const float MIN_PROPERTY_WIDTH = 200;
        private const float MAX_PROPERTY_WIDTH = 400;

        private const float DRAG_LINE_WIDTH = 3;

        private Rect m_PropertyRect = new Rect();
        private Rect m_GroupRect = new Rect();
        private Rect m_DragRect = new Rect();

        private EGUIDragLine m_PropertyDragLine = null;

        private TrackGroupAreaDrawer m_TrackGroupAreaDrawer = null;
        private PropertyAreaDrawer m_PropertyAreaDrawer = null;

        public CutsceneAreaDrawer()
        {
            m_PropertyAreaDrawer = new PropertyAreaDrawer();
            m_TrackGroupAreaDrawer = new TrackGroupAreaDrawer();

            CutsceneEditorData.EditorData.SelectedCutsceneChangedEvent += OnCutsceneChanged;
        }

        private void OnCutsceneChanged()
        {

        }

        public override void OnGUI(Rect rect)
        {
            if(m_PropertyDragLine == null)
            {
                float dragRectX = rect.width - MIN_PROPERTY_WIDTH - DRAG_LINE_WIDTH;
                float dragRectY = rect.y + 3;
                float dragRectWidth = DRAG_LINE_WIDTH;
                float dragRectHeight = rect.height - 6;
                m_DragRect = new Rect(dragRectX, dragRectY, dragRectWidth, dragRectHeight);
                m_PropertyDragLine = new EGUIDragLine(Window, EGUIDirection.Vertical);
            }
            m_DragRect = m_PropertyDragLine.OnGUI(m_DragRect);
            float minDragRectX = rect.width - MAX_PROPERTY_WIDTH - DRAG_LINE_WIDTH;
            float maxDragRectX = rect.width - MIN_PROPERTY_WIDTH - DRAG_LINE_WIDTH;
            if (m_DragRect.x < minDragRectX)
            {
                m_DragRect.x = minDragRectX;
            }
            if (m_DragRect.x > maxDragRectX)
            {
                m_DragRect.x = maxDragRectX;
            }

            float propertyRectX = m_DragRect.x + m_DragRect.width;
            float propertyRectY = rect.y;
            float propertyRectWidth = rect.width - propertyRectX;
            float propertyRectHeight = rect.height;
            m_PropertyRect = new Rect(propertyRectX, propertyRectY, propertyRectWidth, propertyRectHeight);
            EGUI.DrawAreaLine(m_PropertyRect, Color.grey);
            m_PropertyAreaDrawer.OnGUI(m_PropertyRect);

            float groupRectX = rect.x;
            float groupRectY = rect.y;
            float groupRectWidth = m_DragRect.x ;
            float groupRectHeight = rect.height;
            m_GroupRect = new Rect(groupRectX, groupRectY, groupRectWidth, groupRectHeight);
            EGUI.DrawAreaLine(m_GroupRect, Color.grey);
            m_TrackGroupAreaDrawer.OnGUI(m_GroupRect);
        }
    }
}
