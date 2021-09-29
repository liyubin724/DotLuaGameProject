namespace DotEngine.AI.FSM.Editor
{
    public class FSMBlackboardOperation
    {
        public string name;
    }

    public class FSMRemoveBlackboard : FSMBlackboardOperation
    {
    }

    public enum EntityOriginType
    {
        Global = 0,
        Self,
        SelfParent,
        SelfChild,
        SelfDepend,
    }

    public enum EntityFilterType
    {
        Category = 0,
    }

    public class FSMAddBlackboard : FSMBlackboardOperation
    {
        public EntityOriginType originType = EntityOriginType.Self;
        public EntityFilterType filterType = EntityFilterType.Category;
        public string filterValue;

        public string componentName;
        public string componentFieldName;
    }
}
