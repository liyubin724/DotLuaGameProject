using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExtension
{
    public enum EGUIDirection
    {
        Horizontal = 0,
        Vertical,
    }

    public class EGUIDragLine
    {
        private bool m_IsDragging = false;

        private EditorWindow m_Window = null;
        private EGUIDirection m_Direction = EGUIDirection.Horizontal;
        public EGUIDragLine(EditorWindow win, EGUIDirection direction)
        {
            m_Window = win;
            m_Direction = direction;
        }

        public Rect OnGUI(Rect rect)
        {
            if(m_Direction == EGUIDirection.Horizontal)
            {
                if(rect.height<2.0f)
                {
                    rect.height = 2.0f;
                }    

                EGUI.DrawHorizontalLine(rect, Color.grey);

                EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical);
            }
            else if(m_Direction == EGUIDirection.Vertical)
            {
                if(rect.width<2.0f)
                {
                    rect.width = 2.0f;
                }
                EGUI.DrawVerticalLine(rect, Color.grey);

                EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
            }

            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
                {
                    m_IsDragging = true;
                    Event.current.Use();
                }
                else if (m_IsDragging && Event.current.type == EventType.MouseDrag)
                {
                    if(m_Direction == EGUIDirection.Horizontal)
                    {
                        rect.y += Event.current.delta.y;
                    }else if(m_Direction == EGUIDirection.Vertical)
                    {
                        rect.x += Event.current.delta.x;
                    }
                    m_Window.Repaint();
                }
                else if (m_IsDragging && Event.current.type == EventType.MouseUp)
                {
                    m_IsDragging = false;
                    Event.current.Use();
                }
            }

            return rect;
        }

    }
}
