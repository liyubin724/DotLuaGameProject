using System;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.Utilities
{
    public static class SerializedPropertyUtility
    {
        public static bool IsArrayElement(this SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            return propertyPath.IndexOf("[") > 0 && propertyPath.IndexOf("]") > 0;
        }

        public static void AddElement(this SerializedProperty property,UnityEngine.Object obj)
        {
            property.serializedObject.Update();
            {
                property.InsertArrayElementAtIndex(property.arraySize);
                property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = obj;
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        public static void RemoveElementAt(this SerializedProperty property,int index)
        {
            if(property == null)
            {
                throw new ArgumentNullException();
            }
            if(!property.isArray)
            {
                throw new ArgumentException("Property isn't an array");
            }
            if(index<0 || index>=property.arraySize)
            {
                throw new IndexOutOfRangeException();
            }
            var removedProperty = property.GetArrayElementAtIndex(index);
            if (removedProperty.propertyType == SerializedPropertyType.ObjectReference)
            {
                removedProperty.objectReferenceValue = null;
            }
            property.DeleteArrayElementAtIndex(index);

            property.serializedObject.ApplyModifiedProperties();
        }

    }
}
