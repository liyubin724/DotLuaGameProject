using DotEditor.GUIExtension;
using DotEditor.GUIExtension.DataGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Examples
{
    public class DSData
    {
        public string path;
        public bool isDir = false;
    }

    public class DSModel : GridViewModel
    {
        public override bool HasChild(GridViewData data)
        {
            DSData dsData = data.GetData<DSData>();
            if (dsData.isDir)
            {
                return true;
            }else
            {
                return false;
            }
        }

        protected override void OnDataExpand(GridViewData data)
        {
            DSData dsData = data.GetData<DSData>();
            if(dsData.isDir)
            {
                string[] files = Directory.GetFiles(dsData.path, "*.*", SearchOption.TopDirectoryOnly);
                string[] dirs = Directory.GetDirectories(dsData.path, "*", SearchOption.TopDirectoryOnly);
                if(dirs!=null && dirs.Length>0)
                {
                    foreach(var dir in dirs)
                    {
                        AddChildData(data, new GridViewData(dir, new DSData()
                        {
                            path = dir,
                            isDir = true,
                        })) ;
                    }
                }

                if(files!=null && files.Length>0)
                {
                    foreach(var file in files)
                    {
                        AddChildData(data, new GridViewData(file, new DSData()
                        {
                            path = file,
                            isDir = false,
                        }));
                    }
                }
            }
        }
    }

    public class DirectoryShowTreeView : EGUITreeView
    {
        public DirectoryShowTreeView(DSModel model) : base(model)
        {

        }

        protected override void OnDrawRowItem(Rect rect, GridViewData itemData)
        {
            DSData data = itemData.GetData<DSData>();
            if(data.isDir)
            {
                Texture2D texture = EGUIResources.DefaultFolderIcon;
                Rect iconRect = new Rect(rect.x, rect.y, rect.height, rect.height);
                EditorGUI.DrawPreviewTexture(iconRect, texture);
                rect.x += rect.height;
                rect.width -= rect.height;
                EditorGUI.LabelField(rect, data.path);
            }else
            {
                EditorGUI.LabelField(rect, data.path);
            }
        }
    }
}
