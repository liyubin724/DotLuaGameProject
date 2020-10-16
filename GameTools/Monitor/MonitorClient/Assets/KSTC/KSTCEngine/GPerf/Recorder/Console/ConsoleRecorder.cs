using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json;
using System.Text;

namespace KSTCEngine.GPerf.Recorder
{
    public class ConsoleRecorder : GPerfRecorder
    {
        private StringBuilder m_RecordSB = new StringBuilder();
        public override RecorderType Type => RecorderType.Console;

        public override void HandleRecords(Record[] records)
        {
            if(records!=null && records.Length>0)
            {   
                foreach(var record in records)
                {
                    if(!string.IsNullOrEmpty(record.Data))
                    {
                        string json = JsonConvert.SerializeObject(record, Formatting.Indented);
                        m_RecordSB.AppendLine(json);
                    }
                }

                UnityEngine.Debug.Log($"{GPerfConst.LOG_NAME}::ConsoleRecorder->{m_RecordSB.ToString()}");
                m_RecordSB.Clear();
            }
        }
    }
}
