using DotEditor.GUIExtension;
using DotEngine.BD.Datas;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public class ActionAreaDrawer : BDDrawer
    {
        private const float MIN_ACTION_WIDTH = 6;

        private ActionDragType m_DragType = ActionDragType.None;
        public bool IsSelected
        {
            get
            {
                return EditorData.SelectedAction == GetData<ActionData>();
            }
            set
            {
                if (value)
                {
                    EditorData.SelectedAction = GetData<ActionData>();
                }
            }
        }

        private string m_Label = string.Empty;
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(m_Label))
                {
                    Type actionDataType = Data.GetType();
                    var attrs = actionDataType.GetCustomAttributes(typeof(ActionDataAttribute), false);
                    if (attrs != null && attrs.Length > 0)
                    {
                        ActionDataAttribute attr = attrs[0] as ActionDataAttribute;
                        if (attr != null)
                        {
                            m_Label = attr.Label;
                        }
                    }
                }
                return m_Label;
            }
        }

        public override void OnGUI(Rect rect)
        {
            ActionData actionData = GetData<ActionData>();
            if(actionData == null)
            {
                return;
            }
            DrawerSetting setting = DrawerSetting.GetSetting();

            EventActionData eventActionData = actionData as EventActionData;
            DurationActionData durationActionData = actionData as DurationActionData;

            Rect itemRect = Rect.zero;
            itemRect.x = actionData.FireTime * setting.WidthForSecond - setting.ScrollPosX;
            itemRect.y = rect.y;
            itemRect.height = setting.TracklineHeight;
            itemRect.width = MIN_ACTION_WIDTH;

            if(durationActionData != null)
            {
                itemRect.width = Mathf.Max(itemRect.width, durationActionData.DurationTime * setting.WidthForSecond);
            }else if(eventActionData!=null)
            {
                itemRect.x -= itemRect.width * 0.5f;
            }

            GUI.Label(itemRect, Label, IsSelected ? "flow node 6" : "flow node 5");

            Rect leftRect = new Rect(itemRect.x, itemRect.y, MIN_ACTION_WIDTH * 0.5f, itemRect.height);
            Rect rightRect = new Rect(itemRect.x + itemRect.width - MIN_ACTION_WIDTH * 0.5f, itemRect.y, MIN_ACTION_WIDTH * 0.5f, itemRect.height);
            bool isInLeftRect = false;
            bool isInRightRect = false;
            if(durationActionData!=null)
            {
                EditorGUIUtility.AddCursorRect(leftRect, MouseCursor.ResizeHorizontal);
                EditorGUIUtility.AddCursorRect(rightRect, MouseCursor.ResizeHorizontal);

                EGUI.DrawAreaLine(leftRect, Color.blue);
                EGUI.DrawAreaLine(rightRect, Color.blue);

                isInLeftRect = leftRect.Contains(Event.current.mousePosition);
                isInRightRect = rightRect.Contains(Event.current.mousePosition);
            }

            int eventBtn = Event.current.button;
            EventType eventType = Event.current.type;
            bool isContains = itemRect.Contains(Event.current.mousePosition);
            if (eventBtn == 1 && isContains && eventType == EventType.MouseUp)
            {
                IsSelected = true;

                GenericMenu menu = new GenericMenu();
                menu.AddItem(Contents.copyContent, false, () =>
                {

                });
                menu.AddSeparator("");
                menu.AddItem(Contents.deleteContent, false, () =>
                {

                });
                menu.ShowAsContext();

                Event.current.Use();
            }
            else if(eventBtn == 0)
            {
                if (eventType == EventType.MouseDown && isContains)
                {
                    if (durationActionData != null && !durationActionData.IsFixedDuration)   
                    {
                        if (isInLeftRect)
                        {
                            m_DragType = ActionDragType.ItemLeft;
                        }
                        else if (isInRightRect)
                        {
                            m_DragType = ActionDragType.ItemRight;
                        }
                        else
                        {
                            m_DragType = ActionDragType.Item;
                        }
                    }
                    else
                    {
                        m_DragType = ActionDragType.Item;
                    }

                    IsSelected = true;
                    Event.current.Use();
                }
                else if (m_DragType != ActionDragType.None && eventType == EventType.MouseUp)
                {
                    m_DragType = ActionDragType.None;
                    Event.current.Use();
                }
                else if (m_DragType != ActionDragType.None && IsSelected && eventType == EventType.MouseDrag)
                {
                    Vector2 deltaPos = Event.current.delta;
                    float deltaTime = deltaPos.x / setting.WidthForSecond;
                    if (m_DragType == ActionDragType.ItemLeft)
                    {
                        actionData.FireTime += deltaTime;
                        durationActionData.DurationTime += deltaTime;
                    }
                    else if (m_DragType == ActionDragType.ItemRight)
                    {
                        durationActionData.DurationTime += deltaTime;
                    }
                    else if (m_DragType == ActionDragType.Item)
                    {
                        actionData.FireTime += deltaTime;
                    }

                    Event.current.Use();
                }
            }

        }

        enum ActionDragType
        {
            None = 0,
            Item,
            ItemLeft,
            ItemRight,
        }

        class Contents
        {
            public static GUIContent copyContent = new GUIContent("Copy");
            public static GUIContent deleteContent = new GUIContent("Delete");
        }
    }
}
