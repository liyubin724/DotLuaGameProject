using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    public abstract class PropertyControlDrawer : AttrNativeDrawer
    {
        protected PropertyControlDrawer(PropertyControlAttribute attr) : base(attr)
        {
        }

        public abstract void OnStartGUILayout();
        public abstract void OnEndGUILayout();
    }

    public abstract class PropertyLabelDrawer : AttrNativeDrawer
    {
        protected PropertyLabelDrawer(PropertyLabelAttribute attr) : base(attr)
        {
        }

        public abstract string GetLabel();
    }

    public abstract class PropertyDrawer : AttrNativeDrawer
    {
        public NativeDrawerProperty DrawerProperty { get; private set; }

        protected PropertyDrawer(NativeDrawerProperty drawerProperty,PropertyDrawerAttribute attr) : base(attr)
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
