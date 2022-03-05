using DotEngine.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using LogLevel = DotEngine.Log.LogLevel;

namespace DotEditor.Log
{
    public class LogViewerWindow : EditorWindow
    {
        [MenuItem("Game/Log Viewer")]
        public static void Open()
        {
            var win = GetWindow<LogViewerWindow>("Log Viewer");
            win.minSize = new Vector2(400, 300);
            win.Show();
        }

        private static readonly string WatcherAppenderName = "LogViewerAppender";

        private List<LogData> logDatas = new List<LogData>();
        private WatcherAppender watcherAppender;

        private List<LogData> filterLogDatas = new List<LogData>();

        private ListView contentListView;
        private Label stacktreeLabel;
        void OnEnable()
        {
            if (watcherAppender == null)
            {
                watcherAppender = new WatcherAppender(OnLogReceived, WatcherAppenderName);
            }
            if (Application.isPlaying)
            {
                var logMgr = LogManager.CreateMgr();
                if (!logMgr.HasAppender(WatcherAppenderName))
                {
                    logMgr.AddAppender(watcherAppender);
                }
            }
            else
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            LogLevel[] levels = new LogLevel[]
            {
                LogLevel.Info,LogLevel.Warning,LogLevel.Error
            };
            for (int i =0;i<100;i++)
            {
                int levelValue = UnityEngine.Random.Range(0, levels.Length);

                LogData data = new LogData()
                {
                    Time = DateTime.Now,
                    Tag = "Main "+i,
                    Level = levels[levelValue],
                    Message = "Message "+i,
                    Stacktrace = "Stacktrace "+i,
                };
                logDatas.Add(data);
            }
        }

        void CreateGUI()
        {
            CreateToolbarGUI();
            CreateLogContentGUI();
        }

        void CreateToolbarGUI()
        {
            Toolbar toolbar = new Toolbar();
            {
                ToolbarButton clearBtn = new ToolbarButton(()=>
                {
                    logDatas.Clear();
                    contentListView.Refresh();
                });
                clearBtn.text = "Clear";
                toolbar.Add(clearBtn);

                ToolbarSpacer spacer = new ToolbarSpacer();
                spacer.style.flexGrow = 1;
                toolbar.Add(spacer);

                ToolbarSearchField searchField = new ToolbarSearchField();
                toolbar.Add(searchField);
            };
            rootVisualElement.Add(toolbar);
        }

        void CreateLogContentGUI()
        {
            contentListView = new ListView();
            contentListView.style.flexGrow = 1;

            contentListView.itemsSource = logDatas;
            contentListView.reorderable = false;
            contentListView.selectionType = SelectionType.Single;

            contentListView.makeItem = () =>
            {
                return new LogItemViewer();
            };
            contentListView.bindItem = (itemViewer, itemIndex) =>
            {
                LogData data = logDatas[itemIndex];
                (itemViewer as LogItemViewer).SetItemData(data);
            };
            contentListView.onSelectionChange += (selectedEnumerable) =>
            {
                var logData = selectedEnumerable.ToList()[0] as LogData;
                if(logData!=null)
                {
                    stacktreeLabel.text = logData.Stacktrace;
                }
            };

            rootVisualElement.Add(contentListView);

            stacktreeLabel = new Label();
            rootVisualElement.Add(stacktreeLabel);
        }

        private void OnLogReceived(DateTime time,string tag,LogLevel level,string message,string stacktree)
        {
            LogData data = new LogData()
            {
                Time = time,
                Tag = tag,
                Level = level,
                Message = message,
                Stacktrace = stacktree
            };
            logDatas.Add(data);
            contentListView.Refresh();
        }

        //private ListView contentListView;
        //void CreateGUI()
        //{
        //    var root = rootVisualElement;
        //    contentListView = new ListView();
        //    contentListView.style.flexGrow = 1.0f;
        //    Button btn = new Button(() =>
        //    {
        //        OnLogReceived("Test", LogLevel.Error, "FFFFFF", "SSSSSSSSSS");
        //    });
        //    contentListView.itemsSource = logDatas;
        //    contentListView.makeItem = () =>
        //    {
        //        Label label = new Label();
        //        return label;
        //    };
        //    contentListView.bindItem = (e, i) =>{
        //        LogData data = logDatas[i];
        //        (e as Label).text = data.Message;
        //    };
        //    root.Add(contentListView);
        //    root.Add(btn);
        //}

        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                var logMgr = LogManager.CreateMgr();
                if (!logMgr.HasAppender(WatcherAppenderName))
                {
                    logMgr.AddAppender(watcherAppender);
                }
            }
        }
    }
}
