using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.GUIExtension.RList
{
    public class ReorderableListProperty
    {
        private const float ELEMENT_VERTICAL_MARGIN = 4f;
        private ReorderableList m_RList;

        private SerializedProperty m_Property;

        public SerializedProperty Property
        {
            get => m_Property;
            set
            {
                m_Property = value;
                m_RList.serializedProperty = m_Property;
            }
        }

        public ReorderableListProperty(SerializedProperty property)
        {
            m_Property = property;
            CreateList();
        }

        ~ReorderableListProperty()
        {
            m_Property = null;
            m_RList = null;
        }

        private void CreateList()
        {
            m_RList = new ReorderableList(m_Property.serializedObject, m_Property, true, true, true, true);
            m_RList.drawHeaderCallback += OnListDrawHeader;
            m_RList.onCanRemoveCallback += GetListCanRemove;
            m_RList.drawElementCallback += OnListDrawElement;
            m_RList.elementHeightCallback += GetListElementHeight;
        }

        private void OnListDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(new Rect(rect.x + 10, rect.y, rect.width, rect.height), m_Property.displayName);
        }

        private bool GetListCanRemove(ReorderableList list)
        {
            return m_RList.count > 0;
        }

        private void OnListDrawElement(Rect rect, int index, bool active, bool focused)
        {
            SerializedProperty propertyChild = m_Property.GetArrayElementAtIndex(index);
            float propertyHeight = EditorGUI.GetPropertyHeight(propertyChild, GUIContent.none, true);
            Rect indexRect = new Rect(rect.x, rect.y, 20, propertyHeight);
            EditorGUI.LabelField(indexRect, "" + index, EGUIStyles.MiddleCenterLabel);

            indexRect.x += indexRect.width * 2;
            indexRect.width = rect.width - indexRect.width * 2;

            EditorGUI.PropertyField(indexRect, propertyChild, GUIContent.none, true);
        }

        private float GetListElementHeight(int index)
        {
            return Mathf.Max(EditorGUIUtility.singleLineHeight,
                              EditorGUI.GetPropertyHeight(m_Property.GetArrayElementAtIndex(index),
                                                           GUIContent.none,
                                                           true))
                   + ELEMENT_VERTICAL_MARGIN;
        }

        public void OnGUILayout()
        {
            m_RList.DoLayoutList();
        }

        public void OnGUI(Rect rect)
        {
            m_RList.DoList(rect);
        }
    }
}
