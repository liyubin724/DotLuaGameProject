using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    public abstract class GroupData : BDData
    {
        public string Name = string.Empty;
        public List<TrackData> Tracks = new List<TrackData>();
    }
}
