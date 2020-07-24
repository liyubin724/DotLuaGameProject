﻿using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(IntSliderAttribute))]
    public class IntSliderDrawer : PropertyDrawer
    {
        public IntSliderDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(int);
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<IntSliderAttribute>();

            int leftValue = attr.LeftValue;
            int rightValue = attr.RightValue;
            if (!string.IsNullOrEmpty(attr.LeftValueMemberName))
            {
                leftValue = NativeDrawerUtility.GetMemberValue<int>(attr.LeftValueMemberName, DrawerProperty.Target);
            }
            if (!string.IsNullOrEmpty(attr.RightValueMemberName))
            {
                rightValue = NativeDrawerUtility.GetMemberValue<int>(attr.RightValueMemberName, DrawerProperty.Target);
            }

            label = label ?? "";

            int value = DrawerProperty.GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntSlider(label, value, leftValue, rightValue);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
