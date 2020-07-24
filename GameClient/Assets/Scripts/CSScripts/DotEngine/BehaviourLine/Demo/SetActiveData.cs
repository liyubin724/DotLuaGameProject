using DotEngine.BehaviourLine.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BehaviourLine.Demo
{
    [ActionMenu("Set Active",Prefix ="Test")]
    [ActionName(BriefName ="Set Active",DetailName =" Test for Set Active")]
    public class SetActiveData : EventActionData
    {
        public bool IsVisible = false;
        public SetActiveData() :base()
        {

        }
    }
}
