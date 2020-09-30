using DotEngine.Monitor.Recorder;
using DotEngine.Monitor.Sampler;
using System;
using System.Collections.Generic;

namespace DotEditor.Monitor
{
    public class ProfilerModel
    {
        private Action<MonitorSamplerType> m_ModelChanged = null;

        private Dictionary<MonitorSamplerType, ProfilerTabModel> m_TabModelDic = new Dictionary<MonitorSamplerType, ProfilerTabModel>();

        public ProfilerModel(Action<MonitorSamplerType> changedCallback)
        {
            m_ModelChanged = changedCallback;
        }

        public ProfilerTabModel GetTabModel(MonitorSamplerType type)
        {
            if (!m_TabModelDic.TryGetValue(type, out var tabModel))
            {
                if (type == MonitorSamplerType.Log)
                {
                    tabModel = new ProfilerLogTabModel();
                }
                else
                {
                    tabModel = new ProfilerTabModel();
                }
                m_TabModelDic.Add(type, tabModel);
            }
            return tabModel;
        }

        public void AddRecordMessage(ProfilerRecordMessage recordMessage)
        {
            MonitorSamplerType type = recordMessage.Type;
            ProfilerTabModel tabModel = GetTabModel(type);
            foreach (var record in recordMessage.Records)
            {
                tabModel.AddRecord(record);
            }

            m_ModelChanged?.Invoke(type);
        }
    }
}
