using UnityEditor;

namespace DotEditor.GUIExtension.Windows
{
    public abstract class AWindowTabView
    {
        protected EditorWindow window = null;
        protected AWindowTabView(EditorWindow win)
        {
            window = win;
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public abstract void OnGUI();
    }
}
