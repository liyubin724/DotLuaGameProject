using System;
using UnityEngine;

namespace DotEditor.GUIExtension.Toolbar
{
    public abstract class EditorToolbarItem
    {
        public GUIContent LabelContent { get; set; } = GUIContent.none;
        public Action OnClickAction { get; set; } = null;

        protected EditorToolbarItem(Action onClick,GUIContent label)
        {
            OnClickAction = onClick;
            LabelContent = label;
        }

        protected EditorToolbarItem(Action onClick,string label,string tooltip):this(onClick,new GUIContent(label,tooltip))
        {
        }

        protected EditorToolbarItem(Action onClick,Texture icon,string tooltip):this(onClick,new GUIContent(icon,tooltip))
        {
        }

        protected internal abstract float GetItemWidth();
        protected internal abstract void OnItemGUI(Rect rect, GUIStyle style);
    }

    public class EditorToolbarButton : EditorToolbarItem
    {
        public EditorToolbarButton(Action onClick, GUIContent label) : base(onClick, label)
        {
        }

        public EditorToolbarButton(Action onClick, string label, string tooltip) : base(onClick, label, tooltip)
        {
        }

        public EditorToolbarButton(Action onClick, Texture icon, string tooltip) : base(onClick, icon, tooltip)
        {
        }

        protected internal override void OnItemGUI(Rect rect,GUIStyle style)
        {
            if(UnityEngine.GUI.Button(rect, LabelContent, style))
            {
                OnClickAction?.Invoke();
            }
        }

        protected internal override float GetItemWidth()
        {
            return 32.0f;
        }
    }

    public class EditorToolbarToggleButton : EditorToolbarItem
    {
        public bool IsSelected { get; set; } = false;
        public GUIContent SelectedLabelContent { get; set; }

        internal Action<EditorToolbarToggleButton> OnSelectedAction { get; set; } = null;
        public EditorToolbarToggleButton(Action onClick,GUIContent normalLabel,GUIContent selectedLabel,bool isSelcted) : base(onClick,normalLabel)
        {
            SelectedLabelContent = selectedLabel;
            IsSelected = isSelcted;
        }

        public EditorToolbarToggleButton(Action onClick,string normalLabel,string selectedLabel,bool isSelected)
            :this(onClick,new GUIContent(normalLabel),new GUIContent(selectedLabel),isSelected)
        {

        }

        public EditorToolbarToggleButton(Action onClick, string normalLabel,string normalTooltip, string selectedLabel,string selectedTooltip, bool isSelected)
            : this(onClick, new GUIContent(normalLabel,normalTooltip), new GUIContent(selectedLabel,selectedTooltip), isSelected)
        {

        }

        public EditorToolbarToggleButton(Action onClick, Texture normalIcon, Texture selectedIcon, bool isSelected)
            : this(onClick, new GUIContent(normalIcon), new GUIContent(selectedIcon), isSelected)
        {

        }

        public EditorToolbarToggleButton(Action onClick, Texture normalIcon, string normalTooltip, Texture selectedIcon, string selectedTooltip, bool isSelected)
            : this(onClick, new GUIContent(normalIcon, normalTooltip), new GUIContent(selectedIcon, selectedTooltip), isSelected)
        {

        }


        protected internal override float GetItemWidth()
        {
            return 32.0f;
        }

        protected internal override void OnItemGUI(Rect rect, GUIStyle style)
        {
            if(UnityEngine.GUI.Button(rect, IsSelected ? SelectedLabelContent : LabelContent))
            {
                if(!IsSelected)
                {
                    IsSelected = true;
                    OnClickAction?.Invoke();

                    OnSelectedAction?.Invoke(this);
                }
            }
        }
    }

    //public class EditorToolbarDropdownMenu : EditorToolbarItem
    //{
    //    public EditorToolbarDropdownMenu(Action onClick, GUIContent label, int order) : base(onClick, label, order)
    //    {
    //    }

    //    public EditorToolbarDropdownMenu(Action onClick, string label, string tooltip, int order) : base(onClick, label, tooltip, order)
    //    {
    //    }

    //    public EditorToolbarDropdownMenu(Action onClick, Texture icon, string tooltip, int order) : base(onClick, icon, tooltip, order)
    //    {
    //    }

    //    protected internal override void OnItemGUI(Rect rect, GUIStyle style)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
