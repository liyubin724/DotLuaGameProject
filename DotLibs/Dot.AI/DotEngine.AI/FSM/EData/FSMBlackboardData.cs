namespace DotEngine.AI.FSM.Editor
{
    public enum EntityOriginType
    {
        Global = 0,
        Self,
        SelfRelated,
    }

    public enum EntityRelatedType
    {
        Parent = 0,
        Child,
        Depends,
    }

    public enum EntityFilterType
    {
        Category = 0,
    }

    public class FSMBlackboardData
    {
        public string bbName;

        public EntityOriginType originType = EntityOriginType.Self;
        public EntityRelatedType relatedType = EntityRelatedType.Parent;
        public EntityFilterType filterType = EntityFilterType.Category;
        public string filterValue;

        public string componentName;
        public string componentFieldName;
    }
}
