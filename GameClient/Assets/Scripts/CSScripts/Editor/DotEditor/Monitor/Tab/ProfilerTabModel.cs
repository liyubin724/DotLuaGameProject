using DotEditor.GUIExtension.DataGrid;
using DotEngine.Monitor.Sampler;

namespace DotEditor.Monitor
{
    public class ProfilerTabModel : GridViewModel
    {
        public virtual void AddRecord(MonitorRecord record)
        {
            AddData(new GridViewData(record.ToString(), record));
        }
    }

    public class ProfilerLogTabModel : ProfilerTabModel
    {
        public override void AddRecord(MonitorRecord record)
        {
            LogRecord logRecord = (LogRecord)record;
            foreach (var logData in logRecord.Datas)
            {
                AddData(new GridViewData(logData.ToString(), logData));
            }
        }
    }
}
