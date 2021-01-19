using DotEngine.Utilities;
using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class NItemDrawer : NLayoutDrawer
    {
        public SystemObject Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public int ItemIndex { get; private set; } = -1;

        public bool IsArrayItem
        {
            get
            {
                return Field == null && ItemIndex >= 0;
            }
        }

        public string Label
        {
            get
            {
                if (IsArrayItem)
                {
                    return Field != null ? $"{Field.Name}[{ItemIndex}]" : $"{ItemIndex}";
                }
                else
                {
                    return Field.Name;
                }
            }
        }

        public Type TargetType
        {
            get
            {
                return Target?.GetType();
            }
        }

        public Type ValueType
        {
            get
            {
                if (IsArrayItem)
                {
                    return Field != null ? TypeUtility.GetElementTypeInArrayOrList(Field.FieldType) : TypeUtility.GetElementTypeInArrayOrList(Target.GetType());
                }
                else
                {
                    return Field.FieldType;
                }
            }
        }

        public SystemObject Value
        {
            get
            {
                if (IsArrayItem)
                {
                    return ((IList)Target)[ItemIndex];
                }
                else
                {
                    return Field.GetValue(Target);
                }
            }
            set
            {
                if (IsArrayItem)
                {
                    ((IList)Target)[ItemIndex] = value;
                }
                else
                {
                    Field.SetValue(Target, value);
                }

                OnValueChanged?.Invoke(value);
            }
        }

        public event Action<object> OnValueChanged;

        private NLayoutDrawer innerDrawer = null;

        public NItemDrawer(SystemObject target, FieldInfo field)
        {
            Target = target;
            Field = field;

            RefreshDrawer();
        }

        public NItemDrawer(SystemObject target, int index)
        {
            Target = target;
            ItemIndex = index;

            RefreshDrawer();
        }

        private void RefreshDrawer()
        {
            innerDrawer = null;

            if (Target != null && Value != null)
            {
                innerDrawer = NDrawerUtility.GetLayoutDrawer(this);
                if (innerDrawer is NTypeDrawer typeDrawer)
                {
                    typeDrawer.Label = Label;
                    typeDrawer.FieldDrawer = this;
                }
                else if (innerDrawer is NArrayDrawer arrayDrawer)
                {
                    arrayDrawer.Header = Label;
                }else if(innerDrawer is NObjectDrawer objectDrawer)
                {
                    objectDrawer.Header = Label;
                }
            }
        }

        public override void OnGUILayout()
        {
            object value = Value;
            if (value == null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(Label, GUILayout.Width(EditorGUIUtility.labelWidth));
                    if (GUILayout.Button("Create"))
                    {
                        Value = NDrawerUtility.GetTypeInstance(ValueType);

                        RefreshDrawer();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (innerDrawer != null)
            {
                innerDrawer.OnGUILayout();
            }
            else
            {
                EGUI.BeginGUIColor(Color.red);
                {
                    EditorGUILayout.LabelField(Label, $"The drawer of {ValueType} is not found");
                }
                EGUI.EndGUIColor();
            }
        }
    }
}