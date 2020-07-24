using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DotEngine.Lua.Register.RegisterObjectData;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Lua.Register
{
    [CustomTypeDrawer(typeof(ObjectData))]
    public class ObjectDataCustomDrawer : NativeTypeDrawer
    {
        public ObjectDataCustomDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return typeof(ObjectData).IsAssignableFrom(DrawerProperty.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            ObjectData value = DrawerProperty.GetValue<ObjectData>();
            EditorGUI.BeginChangeCheck();
            {
                value.name = EditorGUILayout.TextField("Name", value.name);

                GameObject newGO = (GameObject)EditorGUILayout.ObjectField("Object", value.obj, typeof(GameObject), true);
                if (newGO != value.obj)
                {
                    if (newGO == null)
                    {
                        value.regObj = null;
                        value.typeName = string.Empty;
                    }
                    else
                    {
                        value.obj = newGO;
                        if (string.IsNullOrEmpty(value.name))
                        {
                            value.name = value.obj.name;
                        }
                        if (value.regObj == null)
                        {
                            value.regObj = value.obj;
                            value.typeName = "GameObject";
                        }
                    }
                }

                EditorGUI.BeginDisabledGroup(true);
                {
                    if (value.regObj != null)
                    {
                        EditorGUILayout.ObjectField("Reg Obj", value.regObj, value.regObj.GetType(), true);
                    }
                    else
                    {
                        EditorGUILayout.ObjectField("Reg Obj", value.regObj, typeof(GameObject), true);
                    }
                }
                EditorGUI.EndDisabledGroup();

                if (value.obj == null)
                {
                    EditorGUILayout.LabelField("Type Name", "Null");
                }
                else
                {
                    List<string> componentNames = new List<string>();
                    List<UnityObject> components = new List<UnityObject>();

                    componentNames.Add(typeof(GameObject).Name);
                    components.Add(value.obj);

                    GameObject uObj = value.obj as GameObject;
                    var comArr = uObj.GetComponents<Component>();
                    foreach (var component in comArr)
                    {
                        string componentTypeName = component.GetType().Name;
                        if (componentNames.IndexOf(componentTypeName) < 0)
                        {
                            componentNames.Add(componentTypeName);
                            components.Add(component);
                        }
                    }
                    string[] typeNames = componentNames.ToArray();
                    string newTypeName = EGUILayout.DrawPopup<string>("Type Name", typeNames, typeNames, value.typeName);
                    if (newTypeName != value.typeName)
                    {
                        value.typeName = newTypeName;
                        value.regObj = components[componentNames.IndexOf(newTypeName)];
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
