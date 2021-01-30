using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using DotEngine.BehaviourLine.Action;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public enum ActionDragType
    {
        None = 0,
        Item,
        ItemLeft,
        ItemRight,
    }

    public class ActionDrawer
    {
        private const float MIN_ACTION_WIDTH = 6;

        public ActionData Data { get; private set; }
        public TracklineDrawer ParentDrawer { get; private set; }
        public string BriefName
        {
            get
            {
                ActionNameAttribute attr = Data.GetType().GetCustomAttribute<ActionNameAttribute>();
                if(attr == null)
                {
                    return string.Empty;
                }
                return attr.BriefName;
            }
        }

        public string DetailName
        {
            get
            {
                ActionNameAttribute attr = Data.GetType().GetCustomAttribute<ActionNameAttribute>();
                if(attr == null)
                {
                    return string.Empty;
                }
                return attr.DetailName;
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if(isSelected != value)
                {
                    isSelected = value;
                    if(isSelected)
                    {
                        ParentDrawer.OnActionSelected(this);
                    }
                }
            }
        }

        private DrawerObject dataDrawerObject = null;

        public ActionDrawer(TracklineDrawer drawer)
        {
            ParentDrawer = drawer;
        }

        public void SetData(ActionData data)
        {
            Data = data;

            if(Data!=null)
            {
                dataDrawerObject = new DrawerObject(Data);
            }
        }

        private ActionDragType dragType = ActionDragType.None;
        public void OnDrawGUI(Rect rect)
        {
            LineSetting setting = LineSetting.Setting;

            Rect itemRect = Rect.zero;
            itemRect.x = Data.FireTime * setting.WidthForSecond - setting.ScrollPosX;
            itemRect.y = rect.y;
            itemRect.height = setting.TracklineHeight;
            itemRect.width = MIN_ACTION_WIDTH;

            DurationActionData durationActionData = null;
            if (Data is DurationActionData)
            {
                durationActionData = (DurationActionData)Data;

                itemRect.width = Mathf.Max(itemRect.width, durationActionData.DurationTime * setting.WidthForSecond);
            }else
            {
                itemRect.x -= itemRect.width * 0.5f;
            }
            GUI.Label(itemRect, BriefName, IsSelected ? "flow node 6" : "flow node 5");

            int eventBtn = Event.current.button;
            EventType eventType = Event.current.type;
            bool isContains = itemRect.Contains(Event.current.mousePosition);

            Rect leftRect = new Rect(itemRect.x, itemRect.y, MIN_ACTION_WIDTH * 0.5f, itemRect.height);
            Rect rightRect = new Rect(itemRect.x + itemRect.width - MIN_ACTION_WIDTH * 0.5f, itemRect.y, MIN_ACTION_WIDTH * 0.5f, itemRect.height);

            bool isInLeftRect = false;
            bool isInRightRect = false;

            if (durationActionData != null && !durationActionData.IsFixedDurationTime)
            {
                EGUI.DrawAreaLine(leftRect, Color.yellow);
                EGUI.DrawAreaLine(rightRect, Color.yellow);

                EditorGUIUtility.AddCursorRect(leftRect, MouseCursor.ResizeHorizontal);
                EditorGUIUtility.AddCursorRect(rightRect, MouseCursor.ResizeHorizontal);

                isInLeftRect = leftRect.Contains(Event.current.mousePosition);
                isInRightRect = rightRect.Contains(Event.current.mousePosition);
            }

            if (eventBtn == 0)
            {
                if(eventType == EventType.MouseDown && isContains)
                {
                    if(durationActionData != null)
                    {
                        if(isInLeftRect)
                        {
                            dragType = ActionDragType.ItemLeft;
                        }else if(isInRightRect)
                        {
                            dragType = ActionDragType.ItemRight;
                        }else
                        {
                            dragType = ActionDragType.Item;
                        }
                    }else
                    {
                        dragType = ActionDragType.Item;
                    }

                    IsSelected = true;
                    Event.current.Use();
                }else if(dragType != ActionDragType.None && eventType == EventType.MouseUp)
                {
                    dragType = ActionDragType.None;
                    Event.current.Use();
                }else if(dragType != ActionDragType.None && IsSelected && eventType == EventType.MouseDrag)
                {
                    Vector2 deltaPos = Event.current.delta;
                    float deltaTime = deltaPos.x / setting.WidthForSecond;
                    if(dragType == ActionDragType.ItemLeft)
                    {
                        if (Data.FireTime < durationActionData.DurationTime + Data.FireTime || deltaTime<0)
                        {
                            Data.FireTime += deltaTime;
                            durationActionData.DurationTime -= deltaTime;
                        }
                    }
                    else if(dragType == ActionDragType.ItemRight)
                    {
                        durationActionData.DurationTime += deltaTime;
                    }else if(dragType == ActionDragType.Item)
                    {
                        Data.FireTime += deltaTime;
                    }

                    float timeLength = ParentDrawer.ParentDrawer.Data.TimeLength;
                    if(Data.FireTime <0)
                    {
                        Data.FireTime = 0;
                    }else if(Data.FireTime> timeLength)
                    {
                        Data.FireTime = timeLength;
                    }

                    if(durationActionData!=null)
                    {
                        durationActionData.DurationTime = Mathf.Max(0.0f, durationActionData.DurationTime);

                        float endTime = durationActionData.FireTime + durationActionData.DurationTime;
                        if(endTime> timeLength)
                        {
                            durationActionData.DurationTime = Mathf.Min(durationActionData.DurationTime, timeLength - durationActionData.FireTime);
                        }
                    }

                    Event.current.Use();
                }
            }else if(eventBtn == 1 && isContains && eventType == EventType.MouseUp)
            {
                IsSelected = true;

                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Copy"), false, () =>
                {
                    string actionJson = JsonConvert.SerializeObject(Data, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    });
                    setting.CopiedActionData = actionJson;
                });
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, () =>
                {
                    ParentDrawer.OnActionDelete(this);
                });
                menu.ShowAsContext();

                Event.current.Use();
            }
        }

        public void OnDrawProperty(Rect rect)
        {
            if (!string.IsNullOrEmpty(DetailName))
            {
                EditorGUILayout.LabelField(DetailName, EditorStyles.wordWrappedLabel);
            }
            Type actionType = Data.GetType();
            EditorGUILayout.LabelField(actionType.Name);

            dataDrawerObject.OnGUILayout();
        }

    }
}
