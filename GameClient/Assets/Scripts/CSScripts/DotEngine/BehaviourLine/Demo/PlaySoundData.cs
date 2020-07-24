using DotEngine.BehaviourLine.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BehaviourLine.Demo
{
    [ActionMenu("Play Sound", Prefix = "Test")]
    [ActionName(BriefName = "Play Sound", DetailName = " Test for Play Sound")]
    public class PlaySoundData : DurationActionData
    {
        public int SoundID = 0;
        public PlaySoundData():base()
        {

        }
    }
}
