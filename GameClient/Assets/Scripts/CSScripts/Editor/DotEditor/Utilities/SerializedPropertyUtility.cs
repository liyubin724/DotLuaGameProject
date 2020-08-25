using UnityEditor;

namespace DotEditor.Utilities
{
    public static class SerializedPropertyUtility
    {
        public static void AddElement(this SerializedProperty property,UnityEngine.Object obj)
        {
            property.serializedObject.Update();
            {
                property.InsertArrayElementAtIndex(property.arraySize);
                property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = obj;
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        public static UnityEngine.Object RemoveElementAt(this SerializedProperty property,int index)
        {
            UnityEngine.Object removedObject = null;
            if(index>=0 && index<property.arraySize)
            {
                property.serializedObject.Update();
                {
                    var removedProperty = property.GetArrayElementAtIndex(index);
                    removedObject = removedProperty.objectReferenceValue;
                    if (removedObject != null)
                    {
                        removedProperty.objectReferenceValue = null;
                    }
                    property.DeleteArrayElementAtIndex(index);
                }
                property.serializedObject.ApplyModifiedProperties();
            }
            return removedObject;
        }
    }
}
