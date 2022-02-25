namespace DotEngine.AI.BT.Datas
{
    public enum EBTLogicalOperatorType
    {
        And = 0,
        Or,
    }

    public class BTLogicalConditionData : BTConditionNodeData
    {
        public BTConditionNodeData LeftConditionData;
        public EBTLogicalOperatorType OperatorType = EBTLogicalOperatorType.And;
        public BTConditionNodeData RightConditionData;
    }
}
