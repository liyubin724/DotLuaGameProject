using System.Collections.Generic;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMStateData
    {
        public string name;
        public string pluginScriptPath;

        public List<FSMBlackboardOperation> beforeEnterBBDatas = new List<FSMBlackboardOperation>();
        public List<FSMBlackboardOperation> afterEnterBBDatas = new List<FSMBlackboardOperation>();

        public List<FSMBlackboardOperation> beforeExitBBDatas = new List<FSMBlackboardOperation>();
        public List<FSMBlackboardOperation> afterExitBBDatas = new List<FSMBlackboardOperation>();
    }
}
