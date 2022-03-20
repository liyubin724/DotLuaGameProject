namespace DotEngine.FSM
{
    public abstract class AFSState : IFSState
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        protected FSMachine Machine { get; set; }

        public virtual void DoInitilize(FSMachine machine)
        {
            Machine = machine;
        }

        public virtual void DoEnter(string from) { }

        public abstract void DoUpdate(float deltaTime);

        public virtual void DoLeave(string to) { }

        public virtual void DoDestroy()
        {
            Machine = null;
        }
    }
}
