using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class BehaviourLineWindow : EditorWindow
    {
        [MenuItem("Game/Behaviour Line/Behaviour Editor")]
        static void ShowWin()
        {
            var win = GetWindow<BehaviourLineWindow>();
            win.titleContent = new GUIContent("Behaviour Line");
            win.Show();
        }

        private TimelineDrawer timelineDrawer = null;
        void Awake()
        {
            timelineDrawer = new TimelineDrawer(this);
        }

        void OnGUI()
        {
            timelineDrawer.OnGUI(new Rect(0, 0, position.width, position.height));
        }
    }
}
