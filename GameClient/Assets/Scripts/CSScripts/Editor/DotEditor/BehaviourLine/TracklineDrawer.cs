using DotEngine.BehaviourLine.Action;
using DotEngine.BehaviourLine.Track;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class TracklineDrawer
    {
        public TimelineDrawer ParentDrawer { get; private set; }
        public TracklineData Data { get; private set; }

        private List<ActionDrawer> actionDrawers = new List<ActionDrawer>();

        private int selectedActionIndex = -1;
        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if(isSelected)
                    {
                        ParentDrawer.OnTrackSelected(this);
                    }else
                    {
                        if(selectedActionIndex>=0 && selectedActionIndex<actionDrawers.Count)
                        {
                            actionDrawers[selectedActionIndex].IsSelected = false;
                        }
                        selectedActionIndex = -1;
                    }
                }
            }
        }

        public TracklineDrawer(TimelineDrawer drawer)
        {
            ParentDrawer = drawer;
        }

        public void SetData(TracklineData data)
        {
            Data = data;

            actionDrawers.Clear();

            foreach(var d in data.Actions)
            {
                ActionDrawer drawer = new ActionDrawer(this);
                drawer.SetData(d);

                actionDrawers.Add(drawer);
            }
        }

        public void OnDrawGUI(Rect rect)
        {
            foreach(var drawer in actionDrawers)
            {
                drawer.OnDrawGUI(rect);
            }

            int eventBtn = Event.current.button;
            EventType eventType = Event.current.type;
            Vector2 mPos = Event.current.mousePosition;
            bool isContains = rect.Contains(Event.current.mousePosition);
            if(isContains && eventType == EventType.MouseUp)
            {
                if(eventBtn == 0)
                {
                    IsSelected = true;
                    Event.current.Use();
                }else if (eventBtn == 1)
                {
                    LineSetting setting = LineSetting.Setting;
                    ActionMenu.ShowMenu((actionData) =>
                    {
                        float fireTime = (mPos.x + setting.ScrollPosX) / setting.WidthForSecond;
                        actionData.Index = setting.GetActionIndex();
                        actionData.FireTime = fireTime;

                        OnActionAdded(actionData);
                    });
                    IsSelected = true;
                    Event.current.Use();
                }
            }
        }

        public void OnDrawProperty(Rect rect)
        {
            Data.Name = EditorGUILayout.TextField("Name", Data.Name);

            EditorGUILayout.Space();

            if(selectedActionIndex>=0)
            {
                actionDrawers[selectedActionIndex].OnDrawProperty(rect);
            }
        }

        internal void OnActionDelete(ActionDrawer actionDrawer)
        {
            selectedActionIndex = -1;
            Data.Actions.Remove(actionDrawer.Data);
            actionDrawers.Remove(actionDrawer);

            ParentDrawer.Window.Repaint();
        }

        internal void OnActionAdded(ActionData actionData)
        {
            Data.Actions.Add(actionData);

            ActionDrawer itemDrawer = new ActionDrawer(this);
            itemDrawer.SetData(actionData);
            actionDrawers.Add(itemDrawer);

            ParentDrawer.Window.Repaint();
        }

        internal void OnActionSelected(ActionDrawer actionDrawer)
        {
            int newSelectedIndex = actionDrawers.IndexOf(actionDrawer);
            if(newSelectedIndex!=selectedActionIndex)
            {
                if(selectedActionIndex>=0 && selectedActionIndex<actionDrawers.Count)
                {
                    actionDrawers[selectedActionIndex].IsSelected = false;
                }
                selectedActionIndex = newSelectedIndex;
            }

            if(!IsSelected)
            {
                IsSelected = true;
            }
        }
    }
}
