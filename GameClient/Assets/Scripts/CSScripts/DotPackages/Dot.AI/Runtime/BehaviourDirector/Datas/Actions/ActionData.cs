namespace DotEngine.BD.Datas
{
    public enum ActionCategory
    {
        Debug = 0,
        Animator,
    }

    public abstract class ActionData
    {
        public int UniqueID = -1;
        public bool IsEnable = true;
        public float FireTime = 0.0f;

        public ConditionData Precondition = null;
    }
}
