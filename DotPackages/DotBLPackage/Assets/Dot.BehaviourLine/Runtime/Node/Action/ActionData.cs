using DotEngine.BL.Node.Condition;

namespace DotEngine.BL.Node.Action
{
    public class ActionData : NodeData
    {
        public ConditionData Precondition = null;

        public int Index = -1;
        public float FireTime = 0.0f;
    }
}
