namespace DotEngine.AI.BT
{
    public abstract class ABTNode
    {
        internal BTController Controller { get; set; }
        protected BTContext Context => Controller?.Context;

        protected BTData Data { get; private set; }

        protected T GetData<T>() where T : BTData
        {
            return (T)Data;
        }

        public virtual void DoInitilize(BTData data)
        {
            Data = data;
        }

        public virtual void DoDestroy()
        {
            Data = null;
        }
    }
}
