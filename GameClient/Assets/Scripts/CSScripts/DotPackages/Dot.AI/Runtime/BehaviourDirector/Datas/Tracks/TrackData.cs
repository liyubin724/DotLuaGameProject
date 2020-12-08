using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    public enum TrackCategory
    {

    }

    public class TrackData
    {
        public string Name = string.Empty;
        public DataTarget Target = DataTarget.All;
        public List<ActionData> Actions = new List<ActionData>();
    }
}
