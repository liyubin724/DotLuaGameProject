using DotEditor.GUIExt.Layout;
using DotEngine.Utilities;
using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class NativeFieldDrawer : ILayoutDrawable
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
                    return TypeUtility.GetArrayOrListElementType(Field.FieldType);
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
                    if(list == null)
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

        public NativeFieldDrawer(FieldInfo field, SystemObject target)
        {
            Field = field;
            Target = target;
        }

        internal NativeFieldDrawer(FieldInfo field, SystemObject target,int arrayIndex)
        {
            Field = field;
            Target = target;
            ArrayIndex = arrayIndex;
        }

        private NativeTypeDrawer typeDrawer = null;
        public void OnGUILayout()
        {
            if(typeDrawer == null)
            {
                typeDrawer = NativeUtility.CreateTypeDrawer(ValueType);
            }
            if(typeDrawer!=null)
            {
                float height = typeDrawer.GetHeight();
                Rect rect = EditorGUILayout.GetControlRect(false, height);
                typeDrawer.OnGUI(rect, FieldName, this);
            }
        }
    }
}
