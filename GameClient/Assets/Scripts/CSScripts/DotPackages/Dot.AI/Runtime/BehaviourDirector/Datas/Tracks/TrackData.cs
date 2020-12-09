using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    public enum TrackCategory
    {

    }

    public class TrackData : BDData
    {
        public string Name = string.Empty;
        public BDDataTarget Target = BDDataTarget.All;
        public List<ActionData> Actions = new List<ActionData>();
    }
}
