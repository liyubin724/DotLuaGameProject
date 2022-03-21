namespace DotEngine.FSM
{
    public abstract class AState : IState
    {
        public string Guid { get; set; }
        public string DisplayName { get; set; }
        public bool IsDefault { get; set; }

        protected Machine Machine { get; set; }

        public virtual void DoInitilize(Machine machine)
        {
            Machine = machine;
        }

        public virtual void DoEnter(string from) { }

        public abstract void DoExecute(float deltaTime);

        public virtual void DoLeave(string to) { }

        public virtual void DoDestroy()
        {
            Machine = null;
        }
    }
}
