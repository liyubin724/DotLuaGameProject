using DotEditor.GUIExtension.DataGrid;
using DotEngine.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotEditor.Log
{
    public enum LogSearchCategory
    {
        All,
        Tag,
        Message,
        StackTrace,
    }

    public class LogViewerData
    {
        public static readonly string SEARCH_CATEGORY_ALL = "All";
        public static readonly string SEARCH_CATEGORY_TAG = "Tag";
        public static readonly string SEARCH_CATEGORY_MESSAGE = "Message";
        public static readonly string SEARCH_CATEGORY_STACKTRACE = "StackTrace";

        public readonly string[] SearchCategories = new string[]
        {
            SEARCH_CATEGORY_ALL,
            SEARCH_CATEGORY_TAG,
            SEARCH_CATEGORY_MESSAGE,
            SEARCH_CATEGORY_STACKTRACE,
        };

        private string m_SearchCategory = SEARCH_CATEGORY_ALL;
        public string SearchCategory 
        {
            get
            {
                return m_SearchCategory;
            }
            set
            {
                m_SearchCategory = value;
                FilterLogDatas();
            }
        }

        private string m_SearchText = string.Empty;
        public string SearchText
        {
            get
            {
                return m_SearchText;
            }
            set
            {
                m_SearchText = value;
                FilterLogDatas();
            }
        }

        private Dictionary<LogLevel, bool> logLevelEnableDic = new Dictionary<LogLevel, bool>();
        private Dictionary<LogLevel, int> logLevelCountDic = new Dictionary<LogLevel, int>();

        private List<LogData> logDatas = new List<LogData>();
        public GridViewModel GridViewModel { get;} = new GridViewModel();

        public LogData SelectedLogData = null;

        public Action OnLogDataChanged;

        public LogViewerData()
        {
        }

        public void Reset()
        {
            GridViewModel.Clear();
            logDatas.Clear();
            logLevelCountDic.Clear();
            logLevelEnableDic.Clear();

            OnLogDataChanged?.Invoke();
        }

        public void AddLogData(LogData data)
        {
            logDatas.Add(data);
            IncreaseLogLevelCount(data.Level);

            GridViewModel.AddData(new GridViewData("",data));
            OnLogDataChanged?.Invoke();
        }

        public void AddLogDatas(LogData[] datas)
        {
            foreach(var data in datas)
            {
                logDatas.Add(data);
                IncreaseLogLevelCount(data.Level);

                GridViewModel.AddData(new GridViewData("", data));
            }
            OnLogDataChanged?.Invoke();
        }

        public void FilterLogDatas()
        {
            GridViewModel.Clear();
            foreach (var logData in logDatas)
            {
                if (!GetIsLogLevelEnable(logData.Level))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(SearchText))
                {
                    GridViewModel.AddData(new GridViewData("", logData));
                }
                else
                {
                    if ((SearchCategory == SEARCH_CATEGORY_ALL || SearchCategory == SEARCH_CATEGORY_TAG) && logData.Tag.ToLower().IndexOf(SearchText) >= 0)
                    {
                        GridViewModel.AddData(new GridViewData("", logData));
                    }
                    else if ((SearchCategory == SEARCH_CATEGORY_ALL || SearchCategory == SEARCH_CATEGORY_MESSAGE) && logData.Message.ToLower().IndexOf(SearchText) >= 0)
                    {
                        GridViewModel.AddData(new GridViewData("", logData));
                    }
                    else if ((SearchCategory == SEARCH_CATEGORY_ALL || SearchCategory == SEARCH_CATEGORY_STACKTRACE) && logData.StackTrace.ToLower().IndexOf(SearchText) >= 0)
                    {
                        GridViewModel.AddData(new GridViewData("", logData));
                    }
                }
            }

            OnLogDataChanged?.Invoke();
        }

        public bool GetIsLogLevelEnable(LogLevel level)
        {
            if(!logLevelEnableDic.TryGetValue(level,out var isEnable))
            {
                isEnable = true;
                logLevelEnableDic.Add(level, isEnable);
            }
            return isEnable;
        }

        public void ReverseIsLogLevelEnable(LogLevel level)
        {
            if (logLevelEnableDic.ContainsKey(level))
            {
                logLevelEnableDic[level] = !logLevelEnableDic[level];
            }
            else
            {
                logLevelEnableDic.Add(level, true);
            }

            FilterLogDatas();
        }

        public int GetLogLevelCount(LogLevel level)
        {
            if(!logLevelCountDic.TryGetValue(level,out var count))
            {
                count = 0;
                logLevelCountDic.Add(level, count);
            }
            return count;
        }

        public void IncreaseLogLevelCount(LogLevel level)
        {
            if (logLevelCountDic.ContainsKey(level))
            {
                logLevelCountDic[level] += 1;
            }else
            {
                logLevelCountDic.Add(level, 1);
            }
        }
    }
}
