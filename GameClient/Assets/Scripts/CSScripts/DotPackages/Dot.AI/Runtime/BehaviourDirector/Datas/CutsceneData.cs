using DotEngine.NativeDrawer.Visible;
using DotEngine.NativeDrawer.Property;
using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    public class CutsceneData : BDData
    {
        public string Name = string.Empty;
        [MultilineText(5)]
        public string Desc = string.Empty;
        [FloatSlider(0.0f,99.0f)]
        public float Duration = 0.0f;

        [Hide]
        public List<TrackGroupData> Groups = new List<TrackGroupData>();
    }
}
