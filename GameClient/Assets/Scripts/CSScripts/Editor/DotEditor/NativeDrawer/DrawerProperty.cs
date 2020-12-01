using DotEditor.GUIExtension;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Listener;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Visible;
using DotEngine;
using DotEngine.NativeDrawer;
using DotEngine.NativeDrawer.Layout;
using DotEngine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public class DrawerProperty
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public int ArrayElementIndex { get; private set; } = -1;

        public string FieldName
        {
            get
            {
                if(IsArrayElement)
                {
                    return $"{Field.Name}-{ArrayElementIndex}";
                }
                return Field.Name;
            }
        }

        public Type TargetType
        {
            get
            {
                return Target?.GetType();
            }
        }

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

        private List<ListenerDrawer> listenerDrawers = new List<ListenerDrawer>();

        private List<VisibleDrawer> visibleDrawers = new List<VisibleDrawer>();

        private List<PropertyControlDrawer> controlDrawers = new List<PropertyControlDrawer>();
        private PropertyLabelDrawer labelDrawer = null;
        private PropertyContentDrawer contentDrawer = null;

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
                var drawerAttrs = Field.GetCustomAttributes<DrawerAttribute>();
                foreach (var attr in drawerAttrs)
                {
                    var drawer = DrawerUtility.CreateAttrDrawer(this, attr);
                    if (drawer == null)
                    {
                        DebugLog.Warning("DrawerProperty::Init->drawer not found.attr = " + attr.GetType().Name);
                    }else
                    {
                        if(drawer.GetType().IsSubclassOf(typeof(DecoratorDrawer)))
                        {
                            decoratorDrawers.Add(drawer as DecoratorDrawer);
                        }else if (drawer.GetType().IsSubclassOf(typeof(LayoutDrawer)))
                        {
                            layoutDrawers.Add(drawer as LayoutDrawer);
                        }else if(drawer.GetType().IsSubclassOf(typeof(ListenerDrawer)))
                        {
                            listenerDrawers.Add(drawer as ListenerDrawer);
                        }else if (drawer.GetType().IsSubclassOf(typeof(PropertyControlDrawer)))
                        {
                            controlDrawers.Add(drawer as PropertyControlDrawer);
                        } else if(drawer.GetType().IsSubclassOf(typeof(VisibleDrawer)))
                        {
                            visibleDrawers.Add(drawer as VisibleDrawer);
                        }else if (drawer.GetType().IsSubclassOf(typeof(PropertyLabelDrawer)))
                        {
                            if (labelDrawer != null)
                            {
                                DebugLog.Warning("DrawerProperty::Init->labelDrawer has been found.attr = " + attr.GetType().Name);
                            }else
                            {
                                labelDrawer = drawer as PropertyLabelDrawer;
                            }
                        }else if (drawer.GetType().IsSubclassOf(typeof(PropertyContentDrawer)))
                        {
                            if (contentDrawer != null)
                            {
                                DebugLog.Warning("DrawerProperty::Init->contentDrawer has been found.attr = " + attr.GetType().Name);
                            }
                            else
                            {
                                contentDrawer = drawer as PropertyContentDrawer;
                            }
                        }
                    }
                }
            }

            if(contentDrawer == null)
            {
                contentDrawer = DrawerUtility.CreateCustomTypeDrawer(this);
                if(contentDrawer ==null)
                {
                    if (DrawerUtility.IsTypeSupported(ValueType))
                    {
                        if (TypeUtility.IsStructOrClass(ValueType) && Value != null)
                        {
                            drawerObject = new DrawerObject(Value);
                        }
                    }
                }
            }
        }

        internal void OnGUILayout()
        {
            foreach (var drawer in layoutDrawers)
            {
                LayoutAttribute attr = drawer.GetAttr<LayoutAttribute>();
                if(attr.Occasion == LayoutOccasion.Before)
                {
                    drawer.OnGUILayout();
                }
            }

            bool isVisible = IsVisible();
            if (isVisible)
            {
                foreach (var drawer in decoratorDrawers)
                {
                    drawer.OnGUILayout();
                }

                foreach (var drawer in controlDrawers)
                {
                    drawer.OnStartGUILayout();
                }

                string label = FieldName;
                if(labelDrawer!=null)
                {
                    label = labelDrawer.GetLabel();
                }
                if(!string.IsNullOrEmpty(label))
                {
                    label = UnityEditor.ObjectNames.NicifyVariableName(label);
                }
                if(contentDrawer!=null)
                {
                    contentDrawer.OnGUILayout(label);
                }else if(drawerObject!=null)
                {
                    if (!IsArrayElement)
                    {
                        UnityEditor.EditorGUILayout.LabelField(label);
                        UnityEditor.EditorGUI.indentLevel++;
                        {
                            drawerObject.OnGUILayout();
                        }
                        UnityEditor.EditorGUI.indentLevel--;
                    }
                    else
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
                    if (!DrawerUtility.IsTypeSupported(ValueType))
                    {
                        EGUI.BeginGUIColor(UnityEngine.Color.red);
                        {
                            UnityEditor.EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, $"The type isn't supported.type = {ValueType}");
                        }
                        EGUI.EndGUIColor();
                    }
                    else if (Value == null)
                    {
                        UnityEditor.EditorGUILayout.BeginHorizontal();
                        {
                            UnityEditor.EditorGUILayout.PrefixLabel(label);
                            if (UnityEngine.GUILayout.Button("Create"))
                            {
                                Value = DrawerUtility.CreateInstance(ValueType);

                                if (DrawerUtility.IsTypeSupported(ValueType))
                                {
                                    if (TypeUtility.IsStructOrClass(ValueType) && Value != null)
                                    {
                                        drawerObject = new DrawerObject(Value);
                                    }
                                }
                            }
                        }
                        UnityEditor.EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EGUI.BeginGUIColor(UnityEngine.Color.red);
                        {
                            UnityEditor.EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Unknown Drawer");
                        }
                        EGUI.EndGUIColor();
                    }
                }

                foreach (var drawer in controlDrawers)
                {
                    drawer.OnEndGUILayout();
                }
            }

            foreach (var drawer in layoutDrawers)
            {
                LayoutAttribute attr = drawer.GetAttr<LayoutAttribute>();
                if (attr.Occasion == LayoutOccasion.After)
                {
                    drawer.OnGUILayout();
                }
            }
        }

        private bool IsVisible()
        {
            if (IsArrayElement)
            {
                return true;
            }

            if(visibleDrawers.Count>0)
            {
                bool visible = true;
                foreach(var drawer in visibleDrawers)
                {
                    visible = visible && drawer.IsVisible();
                }
                return visible;
            }else
            {
                return Field.IsPublic;
            }
        }

        internal void ClearArrayElement()
        {
            if (TypeUtility.IsArrayOrList(ValueType))
            {
                if (ValueType.IsArray)
                {
                    Value = DrawerUtility.CreateInstance(ValueType);
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
                object element = DrawerUtility.CreateInstance(TypeUtility.GetArrayOrListElementType(ValueType));
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
            foreach (var drawer in listenerDrawers)
            {
                if (drawer.GetType() == typeof(OnValueChangedDrawer))
                {
                    ((OnValueChangedDrawer)drawer).Execute();
                }
            }
        }
    }
}
