using System.Collections.Generic;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMStateData
    {
        public string name;
        public string pluginScriptPath;

        public List<FSMTranslateConditionData> conditions = new List<FSMTranslateConditionData>();
    }
}
