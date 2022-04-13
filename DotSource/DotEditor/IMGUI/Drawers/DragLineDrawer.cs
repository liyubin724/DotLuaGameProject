using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
{
    public enum DragLineDirection
    {
        Horizontal = 0,
        Vertical,
    }

    public class DragLineDrawer : ILayoutDrawable
    {
        //方向
        private DragLineDirection direction = DragLineDirection.Vertical;
        private EditorWindow window = null;
        private bool isDragging = false;
        private Rect position = Rect.zero;

        //对于不同方向的DragLine限定最小值
        public float LowLimitXY { get; set; } = -1;
        public float UpperLimitXY { get; set; } = -1;

        public float MinX => position.xMin;
        public float MaxX => position.xMax;
        public float MinY => position.yMin;
        public float MaxY => position.yMax;
        public Rect Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public DragLineDrawer(EditorWindow window, DragLineDirection direction = DragLineDirection.Vertical)
        {
            this.window = window;
            this.direction = direction;
        }

        public void OnGUILayout()
        {
            if (direction == DragLineDirection.Horizontal)
            {
                if (position.height < 2.0f)
                {
                    position.height = 2.0f;
                }
                if(LowLimitXY>0.0f && position.y < LowLimitXY)
                {
                    position.y = LowLimitXY;
                }else if(UpperLimitXY>0.0f && position.y+position.height > UpperLimitXY)
                {
                    position.y = UpperLimitXY - position.height;
                }

                EGUI.DrawHorizontalLine(position, Color.grey);
                EditorGUIUtility.AddCursorRect(position, MouseCursor.ResizeVertical);
            }
            else if (direction == DragLineDirection.Vertical)
            {
                if (position.width < 2.0f)
                {
                    position.width = 2.0f;
                }
                if (LowLimitXY > 0.0f && position.x < LowLimitXY)
                {
                    position.x = LowLimitXY;
                }
                else if (UpperLimitXY > 0.0f && position.x + position.width > UpperLimitXY)
                {
                    position.x = UpperLimitXY - position.width;
                }

                EGUI.DrawVerticalLine(position, Color.grey);
                EditorGUIUtility.AddCursorRect(position, MouseCursor.ResizeHorizontal);
            }

            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && position.Contains(Event.current.mousePosition))
                {
                    isDragging = true;
                    Event.current.Use();
                }
                else if (isDragging && Event.current.type == EventType.MouseDrag)
                {
                    if (direction == DragLineDirection.Horizontal)
                    {
                        position.y += Event.current.delta.y;
                    }
                    else if (direction == DragLineDirection.Vertical)
                    {
                        position.x += Event.current.delta.x;
                    }
                    window.Repaint();
                }
                else if (isDragging && Event.current.type == EventType.MouseUp)
                {
                    isDragging = false;
                    Event.current.Use();
                }
            }

        }
    }
}
