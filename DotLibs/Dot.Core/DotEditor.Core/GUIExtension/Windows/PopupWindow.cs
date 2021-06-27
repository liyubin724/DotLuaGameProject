using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExtension.Windows
{
    public abstract class PopupWindowContent
    {
        public PopupWindow Window { get; set; }
        public virtual void OnOpen() { }
        public abstract void OnGUI(Rect rect);
        public virtual void OnClose() { }
    }

    public class PopupWindow : EditorWindow
    {
        public static PopupWindow ShowWin(Rect rect,PopupWindowContent content,bool isDrag = false,bool isCloseWhenLostFocus = false)
        {
            var popupWin = EditorWindow.GetWindow<PopupWindow>();
            popupWin.Init(rect, content, isDrag, isCloseWhenLostFocus);

            return popupWin;
        }

        private PopupWindowContent windowContent;
        private bool isCloseWhenLostFocus = true;
        private bool isDrag = false;
        private Vector2 offset;

        internal void Init(Rect rect,PopupWindowContent content, bool isDrag, bool isCloseWhenLostFocus)
        {
            hideFlags = HideFlags.DontSave;
            wantsMouseMove = true;
            windowContent = content;
            windowContent.Window = this;
            minSize = rect.size;
            position = rect;

            windowContent.OnOpen();

            ShowPopup();
        }

        void OnGUI()
        {
            DrawBackground();
            if(windowContent!=null)
            {
                windowContent.OnGUI(new Rect(2,2,position.size.x-4,position.size.y -4));
            }

            if(isDrag)
            {
                var e = Event.current;
                if (e.button == 0 && e.type == EventType.MouseDown)
                {
                    offset = position.position - UnityEngine.GUIUtility.GUIToScreenPoint(e.mousePosition);
                }

                if (e.button == 0 && e.type == EventType.MouseDrag)
                {
                    var mousePos = UnityEngine.GUIUtility.GUIToScreenPoint(e.mousePosition);
                    position = new Rect(mousePos + offset, position.size);
                }
            }
        }

        private void DrawBackground()
        {
            Rect winRect = new Rect(Vector2.zero, position.size);
            EditorGUI.DrawRect(winRect, EGUIResources.BorderColor);

            Rect backgroundRect = new Rect(Vector2.one, position.size - new Vector2(2f, 2f));
            EditorGUI.DrawRect(backgroundRect, EGUIResources.BackgroundColor);
        }

        private void OnLostFocus()
        {
            if(isCloseWhenLostFocus)
            {
                CloseWindow();
            }
        }

        void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= CloseWindow;
        }

        void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= CloseWindow;
            if(windowContent!=null)
            {
                windowContent.OnClose();
            }
            windowContent = null;
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
