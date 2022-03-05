using DotEngine.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.Log
{
    public class LogViewWindow : EditorWindow
    {
        [MenuItem("Game/Log Viewer")]
        public static void Open()
        {
            var win = GetWindow<LogViewWindow>("Log Viewer");
            win.minSize = new Vector2(400, 300);
            win.Show();
        }

        private static readonly string WatchLogAppenderName = "LogViewWatchAppender";

        private List<LogData> logDatas = new List<LogData>();
        private WatchLogAppender watchLogAppender;

        private ListView contentListView;
        void CreateGUI()
        {
            var root = rootVisualElement;
            contentListView = new ListView();
            contentListView.style.flexGrow = 1.0f;
            Button btn = new Button(() =>
            {
                OnLogReceived("Test", LogLevel.Error, "FFFFFF", "SSSSSSSSSS");
            });
            contentListView.itemsSource = logDatas;
            contentListView.makeItem = () =>
            {
                Label label = new Label();
                return label;
            };
            contentListView.bindItem = (e, i) =>{
                LogData data = logDatas[i];
                (e as Label).text = data.Message;
            };
            root.Add(contentListView);
            root.Add(btn);
        }

        private void CreateToolbar()
        {

        }

        void OnEnable()
        {
            if (watchLogAppender == null)
            {
                watchLogAppender = new WatchLogAppender(OnLogReceived, WatchLogAppenderName);
            }
            if (Application.isPlaying)
            {
                var logMgr = LogManager.CreateMgr();
                if(!logMgr.HasAppender(WatchLogAppenderName))
                {
                    logMgr.AddAppender(watchLogAppender);
                }
            }
            else
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }
        }

        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.EnteredPlayMode)
            {
                var logMgr = LogManager.CreateMgr();
                if (!logMgr.HasAppender(WatchLogAppenderName))
                {
                    logMgr.AddAppender(watchLogAppender);
                }
            }
        }

        private void OnLogReceived(string tag,LogLevel level,string message,string stacktrace)
        {
            logDatas.Add(new LogData()
            {
                Tag = tag,
                Level = level,
                Message = message,
                Stacktrace = stacktrace
            });

            contentListView.Refresh();
        }

    }
}
