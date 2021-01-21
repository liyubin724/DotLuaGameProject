using DotEditor.GUIExt;
using DotEditor.GUIExt.DataGrid;
using DotEditor.GUIExt.Layout;
using DotEditor.GUIExt.NativeDrawer;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    public class CheckerWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Asset Checker")]
        static void ShowWin()
        {
            var win = GetWindow<CheckerWindow>();
            win.titleContent = new GUIContent("Asset Checker");
            win.Show();
        }

        private ToolbarDrawer toolbarDrawer = null;
        private EasyListView configListView = null;
        private DragLineDrawer dragLineDrawer = null;

        private NObjectDrawer checkerDrawer = null;
        private bool isAutoSave = true;
        private int saveInterval = 10;

        private CheckerFileInfo selectedCheckerFileInfo = null;
        private List<CheckerFileInfo> checkerFileInfos = null;
        private double timeSinceStartup = 0.0f;

        private string[] saveIntervalContents = new string[]
        {
            "10s","30s","60s","180s","300s"
        };
        private int[] saveIntervalValues = new int[]
        {
            10,30,60,180,300
        };

        private void OnEnable()
        {
            RefreshConfigs();

            timeSinceStartup = EditorApplication.timeSinceStartup;
            EditorApplication.update += DoUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= DoUpdate;
        }

        private void DoUpdate()
        {
            if(isAutoSave)
            {
                double curTime = EditorApplication.timeSinceStartup;
                if(curTime - timeSinceStartup >= saveInterval)
                {
                    SaveSelectedCheckerFile();
                }
            }else
            {
                timeSinceStartup = EditorApplication.timeSinceStartup;
            }
        }

        private void SaveSelectedCheckerFile()
        {
            if (selectedCheckerFileInfo != null)
            {
                CheckerUtility.SaveCheckerFileInfo(selectedCheckerFileInfo);
            }
            timeSinceStartup = EditorApplication.timeSinceStartup;
        }

        private void SetSelected(CheckerFileInfo fileInfo)
        {
            selectedCheckerFileInfo = fileInfo;
            checkerDrawer = null;
            if(selectedCheckerFileInfo != null)
            {
                checkerDrawer = new NObjectDrawer(selectedCheckerFileInfo.checker);
                checkerDrawer.IsShowBox = true;
                checkerDrawer.IsShowScroll = true;
            }
        }

        private void RefreshConfigs()
        {
            checkerFileInfos = CheckerUtility.ReadCheckerFileInfos();

            selectedCheckerFileInfo = null;
            checkerDrawer = null;

            if (configListView!=null)
            {
                configListView.Clear();
            }else
            {
                configListView = new EasyListView();
                configListView.HeaderContent = "Checker File List";
                configListView.OnSelectedChange = (selected) =>
                {
                    SetSelected((CheckerFileInfo)selected);
                };
            }
            foreach(var cfi in checkerFileInfos)
            {
                configListView.AddItem(Path.GetFileNameWithoutExtension(cfi.filePath), cfi);
            }
        }

        private Rect contentRect;
        private void OnGUI()
        {
            DrawToolbar();
            if(dragLineDrawer == null)
            {
                dragLineDrawer = new DragLineDrawer(this, DragLineDirection.Vertical);
            }
            Rect cRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if (cRect != contentRect && cRect.width > 10 && cRect.height > 10)
            {
                contentRect = cRect;
                dragLineDrawer.Position = new Rect(contentRect.x + 300, contentRect.y, 6, contentRect.height);
            }
            dragLineDrawer.LowLimitXY = 200;
            dragLineDrawer.UpperLimitXY = contentRect.width * 0.8f;
            dragLineDrawer.OnGUILayout();

            Rect listViewRect = new Rect(contentRect.x, contentRect.y, dragLineDrawer.MinX - contentRect.x, contentRect.height);
            // EGUI.DrawAreaLine(listViewRect, Color.blue);
            GUILayout.BeginArea(listViewRect);
            {
                configListView.OnGUILayout();
            }
            GUILayout.EndArea();

            Rect objectRect = new Rect(dragLineDrawer.MaxX, contentRect.y, contentRect.width - dragLineDrawer.MaxX, contentRect.height);
            //  EGUI.DrawAreaLine(objectRect, Color.yellow);
            GUILayout.BeginArea(objectRect);
            {
                EGUILayout.DrawBoxHeader("Checker", EGUIStyles.BoxedHeaderCenterStyle,GUILayout.ExpandWidth(true));
                checkerDrawer?.OnGUILayout();
            }
            GUILayout.EndArea();
        }

        private void DrawToolbar()
        {
            if(toolbarDrawer == null)
            {
                toolbarDrawer = new ToolbarDrawer()
                {
                    LeftDrawable = new HorizontalCompositeDrawer
                    (
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.refreshContent,
                            OnClicked = () =>
                            {
                                RefreshConfigs();
                                Repaint();
                            }
                        },
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.createContent,
                            OnClicked = () =>
                            {
                                string filePath = EditorUtility.SaveFilePanel("Save", CheckerUtility.CHECKER_CONFIG_DIR, "asset_checker", "json");
                                if(!string.IsNullOrEmpty(filePath))
                                {
                                    filePath = filePath.Replace("\\", "/");
                                    CheckerFileInfo cfi = new CheckerFileInfo()
                                    {
                                        filePath = filePath,
                                        checker = new Checker(),
                                    };

                                    CheckerUtility.SaveCheckerFileInfo(cfi);
                                    RefreshConfigs();

                                    configListView.SelectedItem = cfi;
                                    SetSelected(cfi);

                                    Repaint();
                                }
                            }
                        },
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.saveContent,
                            OnClicked = () =>
                            {
                                SaveSelectedCheckerFile();
                            },
                        },
                        new ToolbarButtonDrawer()
                        {
                            Text = Contents.deleteContent,
                            OnClicked = () =>
                            {
                                if(selectedCheckerFileInfo!=null)
                                {
                                    CheckerUtility.DeleteCheckerFileInfo(selectedCheckerFileInfo);
                                }
                                RefreshConfigs();
                                Repaint();
                            }
                        }
                    ),
                    RightDrawable = new HorizontalCompositeDrawer
                    (
                        new ToolbarToggleDrawer()
                        {
                            Text = Contents.autoSaveContent,
                            LabelWidth = 60.0f,
                            Value = isAutoSave,
                            OnValueChanged = (result) =>
                            {
                                isAutoSave = result;
                            }
                        },
                        new PopupDrawer<int>()
                        {
                            Width = 60,
                            EnableFunc = () =>
                            {
                                return isAutoSave;
                            },
                            Contents = saveIntervalContents,
                            Values = saveIntervalValues,
                            Value = saveInterval,
                            OnValueChanged = (value) =>
                            {
                                saveInterval = value;
                            }
                        },new ToolbarButtonDrawer()
                        {
                            Text = Contents.settingContent,
                            OnClicked = () =>
                            {
                                var pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                                CheckerSettingPopupContent.ShowWin(pos);
                            }
                        }
                    ),
                };
            }
            toolbarDrawer.OnGUILayout();
        }

        class Contents
        {
            public static string refreshContent = "Refresh";
            public static string createContent = "Create";
            public static string saveContent = "Save";
            public static string deleteContent = "Delete";
            public static string autoSaveContent = "Auto Save";
            public static string settingContent = "Setting";
        }

    }
}
