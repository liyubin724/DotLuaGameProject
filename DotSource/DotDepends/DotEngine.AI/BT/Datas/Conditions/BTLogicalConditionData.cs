namespace DotEngine.AI.BT
{
    public enum EBTLogicalOperatorType
    {
        And = 0,
        Or,
    }

    public class BTLogicalConditionData : BTConditionNodeData
    {
        public BTConditionNodeData LeftConditionData;
        public BTConditionNodeData RightConditionData;
        public EBTLogicalOperatorType OperatorType = EBTLogicalOperatorType.And;
    }
}
