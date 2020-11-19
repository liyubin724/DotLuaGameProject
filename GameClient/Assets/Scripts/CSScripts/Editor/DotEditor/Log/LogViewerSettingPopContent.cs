using DotEditor.GUIExtension;
using DotEngine.Log;
using DotEngine.Log.Appender;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Log
{
    public class LogViewerSettingPopContent : GUIExtension.Windows.PopupWindowContent
    {
        private LogNetSetting m_Setting = null;
        private Vector2 scrollPos = Vector2.zero;

        public LogViewerSettingPopContent(LogNetSetting setting)
        {
            m_Setting = setting;
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EGUILayout.DrawBoxHeader("Setting", EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    if(m_Setting!=null)
                    {
                        EditorGUI.BeginChangeCheck();
                        {
                            m_Setting.GlobalSetting.GlobalLogLevel = (LogLevel)EditorGUILayout.EnumPopup("Global Log Level:", m_Setting.GlobalSetting.GlobalLogLevel);
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            LogViewer.Viewer.ChangeGlobalLogLevel(m_Setting.GlobalSetting.GlobalLogLevel);
                        }
                        if (m_Setting.LoggerSettings.Count > 0)
                        {
                            EditorGUILayout.LabelField("Logger Log Level:");
                            EGUI.BeginIndent();
                            {
                                foreach (var loggerSetting in m_Setting.LoggerSettings)
                                {
                                    EditorGUI.BeginChangeCheck();
                                    {
                                        EditorGUILayout.LabelField(loggerSetting.Name);
                                        EGUI.BeginIndent();
                                        {
                                            loggerSetting.MinLogLevel = (LogLevel)EditorGUILayout.EnumPopup("Min Log Level:", loggerSetting.MinLogLevel);
                                            loggerSetting.StackTraceLogLevel = (LogLevel)EditorGUILayout.EnumPopup("StackTrace Log Level:", loggerSetting.StackTraceLogLevel);
                                        }
                                        EGUI.EndIndent();
                                    }
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        LogViewer.Viewer.ChangeLoggerLogLevel(loggerSetting.Name, loggerSetting.MinLogLevel, loggerSetting.StackTraceLogLevel);
                                    }
                                }
                            }
                            EGUI.EndIndent();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

    }
}
