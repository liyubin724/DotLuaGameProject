using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Layout;
using DotEngine.NativeDrawer.Listener;
using DotEngine.NativeDrawer.Property;
using DotEngine.NativeDrawer.Verification;
using DotEngine.NativeDrawer.Visible;
using DotEngine.Utilities;
using DotEditor.GUIExtension;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Listener;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Verification;
using DotEditor.NativeDrawer.Visible;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DotEngine.NativeDrawer;

namespace DotEditor.NativeDrawer
{
    public class DrawerProperty
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public int ArrayElementIndex { get; private set; } = -1;

        public bool IsArrayElement
        {
            get
            {
                return ArrayElementIndex >= 0;
            }
        }

        public Type ValueType
        {
            get
            {
                if (IsArrayElement)
                {
                    return TypeUtility.GetArrayOrListElementType(Field.FieldType);
                }

                return Field.FieldType;
            }
        }

        public object Value
        {
            get
            {
                object value = Field.GetValue(Target);
                if(IsArrayElement)
                {
                    return ((IList)value)[ArrayElementIndex];
                }

                return value;
            }
            set
            {
                if(IsArrayElement)
                {
                    IList list = (IList)Field.GetValue(Target);
                    list[ArrayElementIndex] = value;
                }else
                {
                    Field.SetValue(Target, value);
                }

                OnValueChanged();
            }
        }

        public T GetValue<T>()
        {
            return (T)Value;
        }

        private List<DecoratorDrawer> decoratorDrawers = new List<DecoratorDrawer>();
        private List<LayoutDrawer> layoutDrawers = new List<LayoutDrawer>();
        private List<VerificationDrawer> verificationDrawers = new List<VerificationDrawer>();
        private List<VisibleDrawer> visibleDrawers = new List<VisibleDrawer>();
        private List<VisibleCompareDrawer> visibleCompareDrawers = new List<VisibleCompareDrawer>();

        private List<PropertyLabelDrawer> propertyLabelDrawers = new List<PropertyLabelDrawer>();
        private List<PropertyControlDrawer> propertyControlDrawers = new List<PropertyControlDrawer>();
        private List<PropertyDrawer> propertyDrawers = new List<PropertyDrawer>();

        private List<ListenerDrawer> listenerDrawers = new List<ListenerDrawer>();

        private CustomTypeDrawer typeDrawer = null;
        private DrawerObject drawerObject = null;
        internal DrawerProperty(object propertyObject,FieldInfo field)
        {
            Target = propertyObject;
            Field = field;

            Init();
        }

        internal DrawerProperty(object propertyObject, FieldInfo field,int arrayElementIndex)
        {
            Target = propertyObject;
            Field = field;
            ArrayElementIndex = arrayElementIndex;

            Init();
        }

        internal void Init()
        {
            if(!IsArrayElement)
            {
                InitFieldAttr();
            }

            InitDrawer();
        }

        private void InitDrawer()
        {
            typeDrawer = DrawerUtility.CreateDefaultTypeDrawer(this);
            if (typeDrawer == null)
            {
                if(DrawerUtility.IsTypeSupported(ValueType))
                {
                    if (TypeUtility.IsStructOrClass(ValueType) && Value != null)
                    {
                        drawerObject = new DrawerObject(Value);
                    }
                }
            }
        }

        private void InitFieldAttr()
        {
            var drawerAttrs = Field.GetCustomAttributes<DrawerAttribute>();

            var decoratorAttrEnumerable = Field.GetCustomAttributes<DecoratorAttribute>();
            foreach (var attr in decoratorAttrEnumerable)
            {
                decoratorDrawers.Add(DrawerUtility.CreateDecoratorDrawer(this,attr));
            }

            var layoutAttrEnumerable = Field.GetCustomAttributes<LayoutAttribute>();
            foreach (var attr in layoutAttrEnumerable)
            {
                layoutDrawers.Add(DrawerUtility.CreateLayoutDrawer(attr));
            }

            var verificationAttrEnumerable = Field.GetCustomAttributes<VerificationCompareAttribute>();
            foreach (var attr in verificationAttrEnumerable)
            {
                verificationDrawers.Add(DrawerUtility.CreateVerificationDrawer(Target, attr));
            }

            var visibleAttrEnumerable = Field.GetCustomAttributes<VisibleAtrribute>();
            foreach (var attr in visibleAttrEnumerable)
            {
                visibleDrawers.Add(DrawerUtility.CreateVisibleDrawer(attr));
            }

            var visibleCompareAttrEnumerable = Field.GetCustomAttributes<VisibleCompareAttribute>();
            foreach (var attr in visibleCompareAttrEnumerable)
            {
                visibleCompareDrawers.Add(DrawerUtility.CreateVisibleCompareDrawer(Target, attr));
            }

            var propertyLabelAttrEnumerable = Field.GetCustomAttributes<PropertyLabelAttribute>();
            foreach (var attr in propertyLabelAttrEnumerable)
            {
                propertyLabelDrawers.Add(DrawerUtility.CreatePropertyLabelDrawer(attr));
            }

            var propertyControlAttrEnumerable = Field.GetCustomAttributes<PropertyControlAttribute>();
            foreach (var attr in propertyControlAttrEnumerable)
            {
                propertyControlDrawers.Add(DrawerUtility.CreatePropertyControlDrawer(attr));
            }

            var propertyAttrEnumerable = Field.GetCustomAttributes<PropertyDrawerAttribute>();
            foreach (var attr in propertyAttrEnumerable)
            {
                propertyDrawers.Add(DrawerUtility.CreatePropertyDrawer(this, attr));
            }

            var listenerAttrEnumerable = Field.GetCustomAttributes<ListenerAttribute>();
            foreach(var attr in listenerAttrEnumerable)
            {
                listenerDrawers.Add(DrawerUtility.CreateListenerDrawer(Target, attr));
            }
        }

        internal void OnGUILayout()
        {
            bool isVisible = IsVisible();

            foreach (var drawer in layoutDrawers)
            {
                drawer.OnGUILayout();
            }

            if (isVisible)
            {
                foreach (var drawer in decoratorDrawers)
                {
                    drawer.OnGUILayout();
                }

                foreach (var drawer in verificationDrawers)
                {
                    drawer.OnGUILayout();
                }

                foreach (var drawer in propertyControlDrawers)
                {
                    drawer.OnStartGUILayout();
                }

                string label = GetFieldLabel();
                if(!string.IsNullOrEmpty(label))
                {
                    label = UnityEditor.ObjectNames.NicifyVariableName(label);
                }
                if (propertyDrawers.Count == 0)
                {
                    if(typeDrawer != null)
                    {
                        typeDrawer.OnGUILayout(label);
                    }else if(drawerObject!=null)
                    {
                        if(!IsArrayElement)
                        {
                            UnityEditor.EditorGUILayout.LabelField(label);
                            UnityEditor.EditorGUI.indentLevel++;
                            {
                                drawerObject.OnGUILayout();
                            }
                            UnityEditor.EditorGUI.indentLevel--;
                        }else
                        {
                            UnityEditor.EditorGUILayout.BeginHorizontal();
                            {
                                UnityEditor.EditorGUILayout.LabelField(label, UnityEngine.GUILayout.Width(25));
                                UnityEditor.EditorGUILayout.BeginVertical();
                                {
                                    drawerObject.OnGUILayout();
                                }
                                UnityEditor.EditorGUILayout.EndVertical();
                            }
                            UnityEditor.EditorGUILayout.EndHorizontal();
                        }
                    }else if(drawerObject == null)
                    {
                        if(!DrawerUtility.IsTypeSupported(ValueType))
                        {
                            EGUI.BeginGUIColor(UnityEngine.Color.red);
                            {
                                UnityEditor.EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, $"The type isn't supported.type = {ValueType}");
                            }
                            EGUI.EndGUIColor();
                        }else if(Value == null)
                        {
                            UnityEditor.EditorGUILayout.BeginHorizontal();
                            {
                                UnityEditor.EditorGUILayout.PrefixLabel(label);
                                if (UnityEngine.GUILayout.Button("Create"))
                                {
                                    Value = DrawerUtility.CreateDefaultInstance(ValueType);
                                    InitDrawer();
                                }
                            }
                            UnityEditor.EditorGUILayout.EndHorizontal();
                        }else
                        {
                            EGUI.BeginGUIColor(UnityEngine.Color.red);
                            {
                                UnityEditor.EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Unknown Drawer");
                            }
                            EGUI.EndGUIColor();
                        }
                    }
                }
                else
                {
                    foreach(var drawer in propertyDrawers)
                    {
                        drawer.OnGUILayout(label);
                    }
                }

                foreach (var drawer in propertyControlDrawers)
                {
                    drawer.OnEndGUILayout();
                }
            }
        }

        private bool IsVisible()
        {
            if (IsArrayElement)
            {
                return true;
            }

            bool visible = Field.IsPublic;
            if (visibleDrawers.Count > 0)
            {
                visible = visibleDrawers[0].IsVisible();
            }
            else if (visibleCompareDrawers.Count > 0)
            {
                visible = visibleCompareDrawers[0].IsVisible();
            }
            return visible;
        }

        private string GetFieldLabel()
        {
            if(IsArrayElement)
            {
                return "" + ArrayElementIndex;
            }

            string label = Field?.Name;
            foreach (var drawer in propertyLabelDrawers)
            {
                label = drawer.GetLabel();
            }
            return label ?? "";
        }

        internal void ClearArrayElement()
        {
            if (TypeUtility.IsArrayOrList(ValueType))
            {
                if (ValueType.IsArray)
                {
                    Value = DrawerUtility.CreateDefaultInstance(ValueType);
                }
                else
                {
                    ((IList)Value).Clear();
                }
            }
        }

        internal void AddArrayElement()
        {
            if (TypeUtility.IsArrayOrList(ValueType))
            {
                object element = DrawerUtility.CreateDefaultInstance(TypeUtility.GetArrayOrListElementType(ValueType));
                if (ValueType.IsArray)
                {
                    Array array = (Array)Value;
                    ArrayUtility.Add(ref array,element);

                    Value = array;
                }
                else
                {
                    ((IList)Value).Add(element);
                }
            }
        }

        internal void RemoveArrayElementAtIndex(int index)
        {
            if(TypeUtility.IsArrayOrList(ValueType))
            {
                if(ValueType.IsArray)
                {
                    Array array = (Array)Value;
                    ArrayUtility.Remove(ref array, index);

                    Value = array;
                }
                else
                {
                    ((IList)Value).RemoveAt(index);
                }
            }
        }

        private void OnValueChanged()
        {
            foreach(var drawer in listenerDrawers)
            {
                if(drawer.GetType() == typeof(OnValueChangedDrawer))
                {
                    ((OnValueChangedDrawer)drawer).Execute();
                }
            }
        }
    }
}
