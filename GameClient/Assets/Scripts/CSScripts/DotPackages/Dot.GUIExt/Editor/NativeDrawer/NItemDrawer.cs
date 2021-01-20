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
        public NInstanceDrawer ParentDrawer { get; set; }
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
        private bool isNeedRefresh = true;

        public NItemDrawer(SystemObject target, FieldInfo field)
        {
            Target = target;
            Field = field;
        }

        public NItemDrawer(SystemObject target, int index)
        {
            Target = target;
            ItemIndex = index;
        }

        public void Refresh()
        {
            isNeedRefresh = true;
        }

        private void RefreshDrawer()
        {
            innerDrawer = null;

            if (Target != null && Value != null)
            {
                NTypeDrawer typeDrawer = NDrawerUtility.GetTypeDrawerInstance(ValueType);
                if (typeDrawer != null)
                {
                    typeDrawer.Label = Label;
                    typeDrawer.FieldDrawer = this;

                    innerDrawer = typeDrawer;
                }else
                {
                    NInstanceDrawer instanceDrawer = null;
                    if (TypeUtility.IsArrayOrListType(ValueType))
                    {
                        instanceDrawer = new NArrayDrawer(Value);
                    }
                    else if(TypeUtility.IsStructOrClassType(ValueType))
                    {
                        instanceDrawer = new NObjectDrawer(Value);
                    }

                    if(instanceDrawer!=null)
                    {
                        instanceDrawer.Header = Label;
                        instanceDrawer.ItemDrawer = this;

                        instanceDrawer.IsShowInherit = ParentDrawer.IsShowInherit;
                        instanceDrawer.IsShowBox = ParentDrawer.IsShowBox;

                        innerDrawer = instanceDrawer;
                    }
                }
            }
        }

        public override void OnGUILayout()
        {
            if(isNeedRefresh)
            {
                RefreshDrawer();
                isNeedRefresh = false;
            }

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