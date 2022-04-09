using DotEngine.Log;
using DotEngine.UIElements;
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

        private LogDataListViewer m_DataListView;
        private TextField m_StacktraceText;
        private LogLevel m_AliveLevel = LogLevelConst.All;

        void OnEnable()
        {
            allFieldNames = (from fieldInfo in typeof(LogData).GetFields(BindingFlags.Public | BindingFlags.Instance)
                             select fieldInfo.Name).ToList();
            searchedFieldNames.AddRange(allFieldNames);
            searchedFieldNames = searchedFieldNames.Distinct().ToList();
            watcherAppender = new WatcherAppender(WatcherAppenderName);
            watcherAppender.AliveLevel = m_AliveLevel;
            watcherAppender.LogHandler = OnLogReceived;
            if (Application.isPlaying)
            {
                var logMgr = LogManager.GetInstance();
                if (logMgr == null)
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
        }

        void CreateGUI()
        {
            CreateToolbarGUI();
            CreateContentGUI();
        }

        void CreateToolbarGUI()
        {
            Toolbar toolbar = new Toolbar();
            {
                ToolbarButton clearBtn = new ToolbarButton(() =>
                {
                    logDatas.Clear();
                    searchedLogDatas.Clear();
                    m_DataListView.UpdateViewer();
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

                EnumFlagsField levelFlagsField = new EnumFlagsField(m_AliveLevel);
                levelFlagsField.label = "Alive Level";
                levelFlagsField.RegisterValueChangedCallback((callback) =>
                {
                    watcherAppender.AliveLevel = (LogLevel)callback.newValue;
                });
                toolbar.Add(levelFlagsField);
            };
            rootVisualElement.Add(toolbar);
        }

        void CreateContentGUI()
        {
            TwoPaneSplitView splitView = new TwoPaneSplitView();
            splitView.orientation = TwoPaneSplitViewOrientation.Vertical;
            splitView.fixedPaneIndex = 1;
            splitView.fixedPaneInitialDimension = 80;
            rootVisualElement.Add(splitView);

            m_DataListView = new LogDataListViewer();
            m_DataListView.BindedData = searchedLogDatas;
            splitView.Add(m_DataListView);

            VisualElement stacktraceElement = new VisualElement();
            stacktraceElement.ExpandWidth();
            stacktraceElement.SetHeight(80);
            splitView.Add(stacktraceElement);

            m_StacktraceText = new TextField();
            m_StacktraceText.label = null;
            m_StacktraceText.ExpandWidthAndHeight();
            m_StacktraceText.multiline = true;
            m_StacktraceText.isReadOnly = true;
            stacktraceElement.Add(m_StacktraceText);

            m_DataListView.OnDataSelected += (data) =>
            {
                m_StacktraceText.value = data == null ? string.Empty : data.Stacktrace;
            };
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
                m_DataListView?.UpdateViewer();
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
            m_DataListView?.UpdateViewer();
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
