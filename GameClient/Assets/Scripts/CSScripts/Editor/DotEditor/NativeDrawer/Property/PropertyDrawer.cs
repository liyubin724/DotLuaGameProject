using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    public abstract class PropertyControlDrawer : AttrDrawer
    {
        protected PropertyControlDrawer(PropertyControlAttribute attr) : base(attr)
        {
        }

        public abstract void OnStartGUILayout();
        public abstract void OnEndGUILayout();
    }

    public abstract class PropertyLabelDrawer : AttrDrawer
    {
        protected PropertyLabelDrawer(PropertyLabelAttribute attr) : base(attr)
        {
        }

        public abstract string GetLabel();
    }

    public abstract class PropertyDrawer : AttrDrawer
    {
        public DrawerProperty DrawerProperty { get; private set; }

        protected PropertyDrawer(DrawerProperty drawerProperty,PropertyDrawerAttribute attr) : base(attr)
        {
            DrawerProperty = drawerProperty;
        }

        protected abstract bool IsValidProperty();

        public void OnGUILayout(string label)
        {
            if(!IsValidProperty())
            {
                OnDrawInvalidProperty(label);
            }else
            {
                OnDrawProperty(label);
            }
        }

        protected abstract void OnDrawProperty(string label);

        protected virtual void OnDrawInvalidProperty(string label)
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Invalid");
            }
            EGUI.EndGUIColor();
        }

    }
}
