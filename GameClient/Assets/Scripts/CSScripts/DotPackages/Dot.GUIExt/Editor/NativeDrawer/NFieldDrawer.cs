using DotEngine.Utilities;
using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class NFieldDrawer : NLayoutDrawer
    {
        public SystemObject Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public int ArrayIndex { get; private set; } = -1;

        public bool IsArrayItem
        {
            get
            {
                return ArrayIndex >= 0;
            }
        }

        public string FieldName
        {
            get
            {
                if (IsArrayItem)
                {
                    return $"{Field.Name}[{ArrayIndex}]";
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
                    return TypeUtility.GetElementTypeInArrayOrList(Field.FieldType);
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
                SystemObject tValue = Field.GetValue(Target);
                if (tValue != null)
                {
                    if (IsArrayItem)
                    {
                        return ((IList)tValue)[ArrayIndex];
                    }
                }
                return tValue;
            }
            set
            {
                if (IsArrayItem)
                {
                    SystemObject list = Field.GetValue(Target);
                    if (list == null)
                    {
                    }
                    ((IList)list)[ArrayIndex] = value;
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

        public NFieldDrawer(FieldInfo field, SystemObject target)
        {
            Field = field;
            Target = target;

            RefreshFieldInfo();
        }

        internal NFieldDrawer(FieldInfo field, SystemObject target, int arrayIndex):this(field,target)
        {
            ArrayIndex = arrayIndex;
        }

        private void RefreshFieldInfo()
        {
            innerDrawer = null;

            object value = Value;
            if(Target!=null &&Field!=null && value!=null)
            {
                innerDrawer = NativeUtility.GetLayoutDrawer(value);
                if(innerDrawer is NTypeDrawer typeDrawer)
                {
                    typeDrawer.Label = FieldName;
                    typeDrawer.FieldDrawer = this;
                }
            }
        }

        public override void OnGUILayout()
        {
            object value = Value;
            if(value == null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(FieldName,GUILayout.Width(EditorGUIUtility.labelWidth));
                    if(GUILayout.Button("Create"))
                    {
                        Value = NativeUtility.CreateTypeInstance(ValueType);

                        RefreshFieldInfo();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }else if(innerDrawer!=null)
            {
                if(innerDrawer is NInstanceDrawer instanceDrawer)
                {
                    EditorGUILayout.LabelField(FieldName);
                    EGUI.BeginIndent();
                    {
                        instanceDrawer.OnGUILayout();
                    }
                    EGUI.EndIndent();
                }
                else if(innerDrawer is NTypeDrawer typeDrawer)
                {
                    typeDrawer.OnGUILayout();
                }
            }
            else
            {
                EGUI.BeginGUIColor(Color.red);
                {
                    EditorGUILayout.LabelField(FieldName, $"The drawer of {ValueType} is not found");
                }
                EGUI.EndGUIColor();
            }
        }
    }
}
