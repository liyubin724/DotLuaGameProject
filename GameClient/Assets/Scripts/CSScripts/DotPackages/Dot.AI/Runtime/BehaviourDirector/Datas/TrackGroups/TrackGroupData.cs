﻿using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    public abstract class TrackGroupData : BDData
    {
        public List<TrackData> Tracks = new List<TrackData>();
    }
}
