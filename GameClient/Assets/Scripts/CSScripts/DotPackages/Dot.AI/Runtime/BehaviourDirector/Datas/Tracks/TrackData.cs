using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    public enum TrackCategory
    {
        Global = 0,
        Actor,
        Camera,
        Audio,
    }

    public class TrackData : BDData
    {
        public string Name = string.Empty;
        public List<ActionData> Actions = new List<ActionData>();
    }
}
