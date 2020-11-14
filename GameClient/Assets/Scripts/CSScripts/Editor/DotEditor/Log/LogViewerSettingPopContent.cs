using DotEditor.GUIExtension;
using DotEngine.Log;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Log
{
    public class LogViewerSetting
    {
        public LogLevel GlobalLogLevel { get; set; } = LogLevel.On;
        public Dictionary<string, LoggerSetting> LoggerLogLevelDic = new Dictionary<string, LoggerSetting>();

        public class LoggerSetting
        {
            public string Name { get; set; }
            public LogLevel MinLogLevel { get; set; } = LogLevel.On;
            public LogLevel StackTraceLogLevel { get; set; } = LogLevel.Error;
        }
    }

    public class LogViewerSettingPopContent : GUIExtension.Windows.PopupWindowContent
    {
        private LogViewerSetting m_Setting = null;
        private Vector2 scrollPos = Vector2.zero;

        public LogViewerSettingPopContent(LogViewerSetting setting)
        {
            m_Setting = setting;
        }

        protected internal override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EGUILayout.DrawBoxHeader("Setting", EGUIStyles.BoxedHeaderCenterStyle, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        m_Setting.GlobalLogLevel = (LogLevel)EditorGUILayout.EnumPopup("Global Log Level:", m_Setting.GlobalLogLevel);
                    }
                    if(EditorGUI.EndChangeCheck())
                    {

                    }
                    if(m_Setting.LoggerLogLevelDic.Count>0)
                    {
                        EditorGUILayout.LabelField("Logger Log Level:");
                        EGUI.BeginIndent();
                        {
                            foreach(var kvp in m_Setting.LoggerLogLevelDic)
                            {
                                EditorGUI.BeginChangeCheck();
                                {
                                    EditorGUILayout.LabelField(kvp.Value.Name);
                                    EGUI.BeginIndent();
                                    {
                                        kvp.Value.MinLogLevel = (LogLevel)EditorGUILayout.EnumPopup("Min Log Level:", kvp.Value.MinLogLevel);
                                        kvp.Value.StackTraceLogLevel = (LogLevel)EditorGUILayout.EnumPopup("StackTrace Log Level:", kvp.Value.StackTraceLogLevel);
                                    }
                                    EGUI.EndIndent();
                                }
                                if (EditorGUI.EndChangeCheck())
                                {

                                }
                            }
                        }
                        EGUI.EndIndent();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
    }
}
