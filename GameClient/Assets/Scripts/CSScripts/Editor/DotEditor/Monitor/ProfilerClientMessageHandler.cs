using DotEngine.Log;
using DotEngine.Net.Client;
using DotEngine.PMonitor.Recorder;
using DotEngine.PMonitor.Sampler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotEditor.PMonitor
{
    public class ProfilerClientMessageHandler : IClientNetMessageHandler
    {
        public ClientNet Net { get; set; }

        private Dictionary<int, Action<int, object>> messageActionDic = new Dictionary<int, Action<int, object>>();

        public ProfilerClientMessageHandler()
        {
            messageActionDic.Add(ProfilerServerMessageID.OPEN_SAMPLER_RESPONSE, OnOpenSamplerResponse);
            messageActionDic.Add(ProfilerServerMessageID.CLOSE_SAMPLER_RESPONSE, OnCloseSamplerResponse);
            messageActionDic.Add(ProfilerServerMessageID.PUSH_FPS_RECORDS, OnFPSRecords);
            messageActionDic.Add(ProfilerServerMessageID.PUSH_LOG_RECORDS, OnLogRecords);
            messageActionDic.Add(ProfilerServerMessageID.PUSH_SYSTEM_RECORDS, OnSystemRecords);
            messageActionDic.Add(ProfilerServerMessageID.PUSH_MEMORY_RECORDS, OnMemoryRecords);
        }

        public bool OnMessageHanlder(int messageID, object message)
        {
            if (messageActionDic.TryGetValue(messageID, out var action))
            {
                action?.Invoke(messageID, message);
                return true;
            }

            return false;
        }

        private void OnOpenSamplerResponse(int messageID, object message)
        {
            S2C_OpenSamplerResponse response = (S2C_OpenSamplerResponse)message;
            LogUtil.LogDebug("Profiler", "Result = " + response.result);
        }

        private void OnCloseSamplerResponse(int messageID, object message)
        {
        }

        private void OnFPSRecords(int messageID, object message)
        {

        }

        private void OnMemoryRecords(int messageID, object message)
        {
            MemoryRecord[] records = (MemoryRecord[])message;
            if(records!=null && records.Length>0)
            {
                foreach(var record in records)
                {
                    UnityEngine.Debug.Log("MemoryRecord =" + record.TotalAllocatedMemoryLong);
                }
            }
        }

        private void OnLogRecords(int messageID, object message)
        {

        }

        private void OnSystemRecords(int messageID, object message)
        {

        }


    }
}
