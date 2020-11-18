using DotEditor.GUIExtension;
using DotEditor.GUIExtension.DataGrid;
using DotEngine.Log;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Log
{
    public class LogGridView : EGUIGridView
    {
        public event Action<LogData> OnSelectedChanged;

        private Dictionary<LogLevel, Color> m_LogColorDic = new Dictionary<LogLevel, Color>();

        public LogGridView(GridViewModel model) : base(model, GetHeader())
        {
            treeView.multiColumnHeader.ResizeToFit();

            m_LogColorDic.Add(LogLevel.Trace, Color.grey);
            m_LogColorDic.Add(LogLevel.Debug, Color.white);
            m_LogColorDic.Add(LogLevel.Info, Color.cyan);
            m_LogColorDic.Add(LogLevel.Warning, Color.yellow);
            m_LogColorDic.Add(LogLevel.Error, Color.red);
            m_LogColorDic.Add(LogLevel.Fatal, Color.red);
        }

        static GridViewHeader GetHeader()
        {
            GridViewHeader header = new GridViewHeader();
            header.AddColumn(new GridViewColumn("Level")
            {
                Width = 64,
                AutoResize = false,
            });
            header.AddColumn(new GridViewColumn("Time")
            {
                Width = 160,
                AutoResize = false,
            });

            header.AddColumn(new GridViewColumn("Tag")
            {
                Width = 120,
                AutoResize = false,
            });

            header.AddColumn(new GridViewColumn("Message")
            {
                AutoResize = true,
            });
            header.AddColumn(new GridViewColumn("StackTrace")
            {
                AutoResize = true,
            });

            return header;
        }

        protected override float GetRowHeight(GridViewData itemData)
        {
            float minHeight = 32;

            return minHeight;
        }

        protected override void OnItemSelectedChanged(GridViewData[] itemDatas)
        {
            if(itemDatas!=null && itemDatas.Length>0)
            {
                OnSelectedChanged(itemDatas[0].Userdata as LogData);
            }else
            {
                OnSelectedChanged(null);
            }
        }

        protected override void OnDrawColumnItem(Rect rect, int columnIndex, GridViewData columnItemData)
        {
            LogData logData = columnItemData.Userdata as LogData;

            Color textColor = Color.white;
            if (m_LogColorDic.TryGetValue(logData.Level, out textColor))
            {

            }
            EGUI.BeginGUIColor(textColor);
            {
                if (columnIndex == 0)
                {
                    GUI.Label(rect, logData.Level.ToString());
                }
                else if (columnIndex == 1)
                {
                    GUI.Label(rect, logData.Time.ToString());
                }
                else if (columnIndex == 2)
                {
                    GUI.Label(rect, logData.Tag);
                }
                else if (columnIndex == 3)
                {
                    GUI.Label(rect, logData.Message);
                }
                else if (columnIndex == 4)
                {
                    GUI.Label(rect, logData.StackTrace);
                }
            }
            EGUI.EndGUIColor();
           
        }
    }
}
