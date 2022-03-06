using DotEngine.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private List<string> allFieldNames = new List<string>();
        private List<string> searchedFieldNames = new List<string>();
        private string searchText = string.Empty;
        private List<LogData> searchedLogDatas = new List<LogData>();

        private ListView contentListView;
        private TextField stacktraceLabel;
        void OnEnable()
        {
            allFieldNames = (from fieldInfo in typeof(LogData).GetFields(BindingFlags.Public | BindingFlags.Instance)
                             select fieldInfo.Name).ToList();
            searchedFieldNames.AddRange(allFieldNames);
            searchedFieldNames = searchedFieldNames.Distinct().ToList();
            watcherAppender = new WatcherAppender(OnLogReceived, WatcherAppenderName);
            if (Application.isPlaying)
            {
                var logMgr = LogManager.GetInstance();
                if(logMgr == null)
                {
                    logMgr = LogManager.CreateMgr();
                }
                logMgr.RemoveAppender(WatcherAppenderName);
                logMgr.AddAppender(watcherAppender);
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
            for (int i = 0; i < 100; i++)
            {
                int levelValue = UnityEngine.Random.Range(0, levels.Length);

                LogData data = new LogData()
                {
                    Time = DateTime.Now,
                    Tag = "Main " + i,
                    Level = levels[levelValue],
                    Message = "Message " + i,
                    Stacktrace = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFStacktrace " + i,
                };
                logDatas.Add(data);
            }

            UpdateSearchedDatas();
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
                ToolbarButton clearBtn = new ToolbarButton(() =>
                {
                    logDatas.Clear();
                    searchedLogDatas.Clear();
                    contentListView.Refresh();
                });
                clearBtn.text = "Clear";
                toolbar.Add(clearBtn);

                ToolbarSpacer spacer = new ToolbarSpacer();
                spacer.style.flexGrow = 1;
                toolbar.Add(spacer);

                ToolbarPopupSearchField searchField = new ToolbarPopupSearchField();
                DropdownMenu searchMenu = searchField.menu;
                List<string> allFieldNames = (from fieldInfo in typeof(LogData).GetFields(BindingFlags.Public | BindingFlags.Instance)
                                              select fieldInfo.Name).ToList();
                foreach (var fieldName in allFieldNames)
                {
                    searchMenu.AppendAction(
                            fieldName,
                            (action) =>
                            {
                                if (searchedFieldNames.IndexOf(action.name) >= 0)
                                {
                                    searchedFieldNames.Remove(action.name);
                                }
                                else
                                {
                                    searchedFieldNames.Add(action.name);
                                }
                            },
                            (action) =>
                            {
                                if (searchedFieldNames.Count > 0 && searchedFieldNames.IndexOf(action.name) >= 0)
                                {
                                    return DropdownMenuAction.Status.Checked;
                                }
                                else
                                {
                                    return DropdownMenuAction.Status.Normal;
                                }
                            },
                            fieldName);
                }
                searchField.RegisterValueChangedCallback((callback) =>
                {
                    searchText = callback.newValue;
                    UpdateSearchedDatas();
                });
                toolbar.Add(searchField);

                EnumFlagsField levelFlagsField = new EnumFlagsField(watcherAppender.AliveLevel);
                levelFlagsField.RegisterValueChangedCallback((callback) =>
                {
                    watcherAppender.AliveLevel = (LogLevel)callback.newValue;
                });
                toolbar.Add(levelFlagsField);
            };
            rootVisualElement.Add(toolbar);
        }

        void CreateLogContentGUI()
        {
            contentListView = new ListView();
            contentListView.style.flexGrow = 1;

            contentListView.itemsSource = searchedLogDatas;
            contentListView.reorderable = false;
            contentListView.selectionType = SelectionType.Single;

            contentListView.makeItem = () =>
            {
                return new LogItemViewer();
            };
            contentListView.bindItem = (itemViewer, itemIndex) =>
            {
                LogData data = contentListView.itemsSource[itemIndex] as LogData;
                (itemViewer as LogItemViewer).SetItemData(data);
            };
            contentListView.onSelectionChange += (selectedEnumerable) =>
            {
                var logData = selectedEnumerable.ToList()[0] as LogData;
                if (logData != null)
                {
                    stacktraceLabel.value = logData.Stacktrace;
                }
            };
            stacktraceLabel = new TextField();
            stacktraceLabel.isReadOnly = true;
            stacktraceLabel.style.whiteSpace = WhiteSpace.Normal;
            stacktraceLabel.style.minHeight = 60;

            TwoPaneSplitView splitView = new TwoPaneSplitView(1, 150, TwoPaneSplitViewOrientation.Vertical);
            splitView.Add(contentListView);
            splitView.Add(stacktraceLabel);
            rootVisualElement.Add(splitView);
        }

        private void OnLogReceived(DateTime time, string tag, LogLevel level, string message, string stacktree)
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

            if (IsMatchSearch(data))
            {
                searchedLogDatas.Add(data);
                contentListView.Refresh();
            }
        }

        private void UpdateSearchedDatas()
        {
            searchedLogDatas.Clear();
            foreach (var data in logDatas)
            {
                if (IsMatchSearch(data))
                {
                    searchedLogDatas.Add(data);
                }
            }
            contentListView?.Refresh();
        }

        private bool IsMatchSearch(LogData data)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            if (searchedFieldNames.Count == 0)
            {
                return false;
            }

            foreach (var fieldName in searchedFieldNames)
            {
                FieldInfo field = typeof(LogData).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
                if (field != null)
                {
                    string strValue = field.GetValue(data).ToString();
                    if (strValue.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                var logMgr = LogManager.GetInstance();
                if (logMgr == null)
                {
                    logMgr = LogManager.CreateMgr();
                }
                logMgr.RemoveAppender(WatcherAppenderName);
                logMgr.AddAppender(watcherAppender);
            }
        }
    }
}
