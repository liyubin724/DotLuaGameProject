using DotEngine.Monitor.Recorder;
using DotEngine.Monitor.Sampler;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Monitor
{
    public class MonitorProfilerModel
    {
        private Dictionary<MonitorSamplerType, List<MonitorRecord>> recordDic = new Dictionary<MonitorSamplerType, List<MonitorRecord>>();

        public void AddRecordMessage(ProfilerRecordMessage recordMessage)
        {
            MonitorSamplerType type = recordMessage.Type;
            if(!recordDic.TryGetValue(type,out var dataList))
            {
                dataList = new List<MonitorRecord>();
                recordDic.Add(type, dataList);
            }
            dataList.AddRange(recordMessage.Records);
        }

        public MonitorRecord[] GetLastRecords(MonitorSamplerType type,int count)
        {
            if (recordDic.TryGetValue(type, out var dataList))
            {
                if(count<=0)
                {
                    count = dataList.Count;
                }else
                {
                    count = Mathf.Min(count, dataList.Count);
                }

                MonitorRecord[] result = new MonitorRecord[count];
                for(int i =0;i<count;++i)
                {
                    MonitorRecord record = dataList[dataList.Count - 1 - i];
                    result[count - 1 - i] = record;
                }
                return result;
            }
            return new MonitorRecord[0];
        }

    }
}
