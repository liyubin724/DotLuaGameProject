using System.Collections.Generic;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMData
    {
        public string name;

        public List<FSMBlackboardData> bbDatas = new List<FSMBlackboardData>();
        public List<FSMStateData> stateDatas = new List<FSMStateData>();
    }
}
