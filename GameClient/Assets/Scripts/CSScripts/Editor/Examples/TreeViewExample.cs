using DotEditor.GUIExtension.DataGrid;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Examples
{
    public class TreeViewExample : EditorWindow
    {
        [MenuItem("FTest/GUI Extension/TreeView Example")]
        public static void ShowWin()
        {
            var win = TreeViewExample.GetWindow<TreeViewExample>();
            win.titleContent = new GUIContent("TreeView Example");
            win.Show();
        }

        private void OnGUI()
        {
            OnGUI1();
            OnGUI2();
            OnGUI3();
        }

        #region Sample List View
        private EGUISampleListView sampleListView = null;
        private string selectedItem = string.Empty;
        private void OnGUI1()
        {
            if(sampleListView == null)
            {
                sampleListView = new EGUISampleListView()
                {
                    ShowSeparator = true,
                    HeaderContent = "Example",
                    OnSelectedChange = (fileName) =>
                     {
                         selectedItem = (string)fileName;
                     },
                };

                string[] files = Directory.GetFiles("F:/", "*.*", SearchOption.TopDirectoryOnly);

                sampleListView.AddItems(files);
            }
            sampleListView.OnGUILayout();

            EditorGUILayout.LabelField($"Current Selected : {selectedItem}");
        }
        #endregion

        #region Extension TreeView

        private DirectoryShowTreeView exTreeView = null;
        private void OnGUI2()
        {
            if(exTreeView == null)
            {
                exTreeView = new DirectoryShowTreeView(new DSModel())
                {
                    HeaderContent = "Expand Tree View"
                };

                string fPath = "F:/";
                string[] files = Directory.GetFiles(fPath, "*.*", SearchOption.TopDirectoryOnly);
                string[] dirs = Directory.GetDirectories(fPath, "*", SearchOption.TopDirectoryOnly);
                if (dirs != null && dirs.Length > 0)
                {
                    foreach (var dir in dirs)
                    {
                        exTreeView.ViewModel.AddData(new GridViewData(dir, new DSData()
                        {
                            path = dir,
                            isDir = true,
                        }));
                    }
                }

                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        exTreeView.ViewModel.AddData(new GridViewData(file, new DSData()
                        {
                            path = file,
                            isDir = false,
                        }));
                    }
                }

                exTreeView.Reload();
            }

            exTreeView.OnGUILayout();
        }
        #endregion

        #region GridView
        private FileShowGridView gridView = null;
        private void OnGUI3()
        {
            if(gridView == null)
            {
                gridView = new FileShowGridView(new GridViewModel(),new GridViewHeader(new string[] {"File","Time","Size" }))
                {
                    HeaderContent = "File Grid View"
                };

                string fPath = "F:/";
                string[] files = Directory.GetFiles(fPath, "*.*", SearchOption.TopDirectoryOnly);

                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        FileInfo fInfo = new FileInfo(file);

                        gridView.ViewModel.AddData(new GridViewData(file, new FileInfoData()
                        {
                            path = file,
                            createDatetime = fInfo.CreationTime.ToString(),
                            fileSize = fInfo.Length
                        })) ;
                    }
                }

                gridView.Reload();
            }
            gridView.OnGUILayout();
        }
        #endregion
    }

}

