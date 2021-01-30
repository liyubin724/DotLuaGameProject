using DotEngine.BehaviourLine.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BehaviourLine.Demo
{
    [ActionItemBindData(typeof(PlaySoundData))]
    public class PlaySoundItem : DurationActionItem
    {
        public PlaySoundItem():base()
        {
        }

        public override void DoEnter()
        {
            throw new NotImplementedException();
        }

        public override void DoExit()
        {
            throw new NotImplementedException();
        }

        public override void DoUpdate(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
