using System.Collections.Generic;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMData
    {
        public string name;

        public string defaultStateName;
        public List<FSMBlackboardOperation> bbOperations = new List<FSMBlackboardOperation>();
        public List<FSMStateData> states = new List<FSMStateData>();
        public List<FSMTransition> transitions = new List<FSMTransition>();
    }
}
